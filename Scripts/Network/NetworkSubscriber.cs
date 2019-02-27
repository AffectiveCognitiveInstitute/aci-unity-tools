// <copyright file=NetworkReceiver.cs/>
// <copyright>
//   Copyright (c) 2018, Affective & Cognitive Institute
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software andassociated documentation files
//   (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
//   merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//   OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
//   IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// </copyright>
// <license>MIT License</license>
// <main contributors>
//   Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>11/21/2018 13:28</date>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aci.Unity.Events;
using Aci.Unity.Logging;
using Aci.Unity.Util;
using Newtonsoft.Json;
using UnityEngine;

namespace Aci.Unity.Network
{
    /// <summary>
    ///     NetworkSubscriber is a basic network communication client receiving json messages via a UDP connection.
    ///     Messages will be deserialized into class instances implementing <see cref="INetworkPackage"/>.
    ///     To handle a new package type an instance of the type has to be bound via Zenject to <see cref="INetworkPackage"/>.
    ///     <example>Container.Bind{INetworkPackage}().To{ConcreteInterface}().FromNew();</example>
    ///     Messages are handled via <see cref="INetworkPackageHandler"/>.
    ///     Custom handlers can be derived from <see cref="NetworkPackageHandlerBase{T}"/>. They will then be automatically be added to a <see cref="NetworkPackageHandlerRegistry"/> via Zenject.
    /// </summary>
    public class NetworkSubscriber : MonoBehaviour
    {
        // package types that have been registered
        private Dictionary<string, Type> m_PackageTypes = new Dictionary<string, Type>();
        // package handlers that have been registered
        private NetworkPackageHandlerRegistry m_PackageRegistry;
        // current event manager instance
        private IAciEventManager m_EventBroker;
        // cancellation token source for interrupting network listening
        private CancellationTokenSource m_Cts;

        // running indicator
        private bool m_Running = false;
        // check for last ping, TODO: move to separate handler handling multiple ips
        private float m_LastPingReceived;

        /// <summary>
        ///     UDP port packages should be received on.
        /// </summary>
        [Tooltip("Port to listen for inbound messages.")]
        public int port = 20000;

        /// Zenject injection method
        [Zenject.Inject]
        private void Construct(IAciEventManager eventBroker, NetworkPackageHandlerRegistry packageRegistry, List<INetworkPackage> packages)
        {
            m_EventBroker = eventBroker;
            m_PackageRegistry = packageRegistry;
            // register packages supplied via installers
            foreach (INetworkPackage pkg in packages)
            {
                if (m_PackageTypes.ContainsKey(pkg.call))
                    continue;
                m_PackageTypes[pkg.call] = pkg.GetType();
            }
        }

        /// <summary>
        ///     Unity Start event handler.
        /// </summary>
        private void Start()
        {
            Initialize();
        }

        /// <summary>
        ///     Called on GameObject disabled.
        ///     Stops receiving data.
        /// </summary>
        private void OnEnable()
        {
            Initialize();
        }

        /// <summary>
        ///     Called on GameObject disabled.
        ///     Stops receiving data.
        /// </summary>
        private void OnDisable()
        {
            if (m_Cts == null)
                return;
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        /// <summary>
        ///     Initializes and starts the receiver instance.
        /// </summary>
        private void Initialize()
        {
            if (!isActiveAndEnabled || m_Running)
                return;
            if (!Application.runInBackground)
                Application.runInBackground = true;
            m_Cts = new CancellationTokenSource();
            Task.Run(() => ReceiveData(m_Cts.Token));
            m_Running = true;
        }

        /// <summary>
        ///     Async network listener loop.
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/> that is used to interrupt listening process.</param>
        /// <returns></returns>
        private async Task ReceiveData(CancellationToken token)
        {
            using (UdpClient client = new UdpClient(port))
            {
                AciLog.LogFormat(LogType.Log, GetType().ToString(), "Starting to receive data on port {0}.", port);
                while (!token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    string data = Encoding.UTF8.GetString(result.Buffer);
                    GetData(data);
                }
                AciLog.Log(GetType().ToString(), "Stopped listening for new data.");
            }
            m_Running = false;
        }

        /// <summary>
        ///     Message deserialization an initiation of handling methods.
        /// </summary>
        /// <param name="serializedData">Message string containing data.</param>
        private void GetData(string serializedData)
        {
            // don't need to handle empty messages
            if (string.IsNullOrEmpty(serializedData))
                return;
            try
            {
                // convert package string into stub
                NetworkPackageStub stub = JsonConvert.DeserializeObject<NetworkPackageStub>(serializedData);
                // check if calltype belongs to a known message type
                Type type;
                if (!m_PackageTypes.TryGetValue(stub.call, out type))
                {
                    AciLog.LogFormat(LogType.Error, GetType().ToString(), "Can't handle NetworkPackage with calltype \"{0}\".", stub.call);
                    return;
                }
                // deserialize into package instance
                INetworkPackage package = JsonConvert.DeserializeObject(serializedData, type) as INetworkPackage;
                // call handler mathods
                UnityMainThreadDispatcher.Instance().Enqueue(() => m_PackageRegistry.Handle(package));
            }
            catch (Exception e)
            {
                AciLog.LogException(e);
            }
        }
        
        /// <summary>
        ///     Executes until either a ping was received in the last timeframe of length delta time or the maxWait time was
        ///     reached.
        /// </summary>
        /// <param name="maxWait">Maximum wait time in seconds.</param>
        /// <param name="maxDelta">Delta time threashold.</param>
        /// <returns>True if last time received below threashold, false if wait time reached.</returns>
        public async Task<bool> WasPinged(int maxWait, float startTime, float maxDelta)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.CancelAfter(maxWait * 1000);

            try
            {
                await awaitPing(startTime, maxDelta, cancelTokenSource.Token);
                return true;
            }
            catch (OperationCanceledException c)
            {
                //timeout, return false
                return false;
            }
        }

        private async Task awaitPing(float startTime, float maxDelta, CancellationToken token)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                token.ThrowIfCancellationRequested();
                if (Math.Abs(startTime + (float)watch.Elapsed.TotalSeconds - m_LastPingReceived) <= maxDelta)
                    return;
            }
        }
    }
}