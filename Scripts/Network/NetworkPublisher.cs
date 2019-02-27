// <copyright file=NetworkPublisher.cs/>
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
// <date>11/20/2018 17:43</date>

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Aci.Unity.Logging;
using Newtonsoft.Json;
using UnityEngine;

namespace Aci.Unity.Network
{
    /// <summary>
    ///     NetworkPublisher is a basic network communication client sending json messages via UDP.
    ///     Messages will be serialized from class instances implementing <see cref="INetworkPackage"/>.
    ///     Messages can be sent to multiple receivers by specifying one ip and one port per receiving client.
    /// </summary>
    public class NetworkPublisher : MonoBehaviour
                                  , INetworkPublisher
    {
        private UdpClient _client;

        /// <summary>
        ///     <see cref="IPEndPoint" /> for all specified connections.
        /// </summary>
        private IPEndPoint[] _remoteEndPoints;

        /// <summary>
        ///     Target receiving IP adresses
        /// </summary>
        public string[] ips = { };

        /// <summary>
        ///     Outbound port
        /// </summary>
        public int[] ports = { };

        void Start()
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes the network connection for sending data
        /// </summary>
        private void Initialize()
        {
            if (ips.Length != ports.Length)
                AciLog.LogError(GetType().Name,
                                           "IP/Port mismatch. Please check the configuration of the NetworkPublisher.");

            _remoteEndPoints = new IPEndPoint[ips.Length];
            for (int i = 0; i < ips.Length; ++i)
            {
                _remoteEndPoints[i] = new IPEndPoint(IPAddress.Parse(ips[i]), ports[i]);
                AciLog.Log(GetType().Name, "Sending packages to " + ips[i] + " : " + ports[i]);
            }

            _client = new UdpClient();
        }

        /// <summary>
        ///     Sends a <see cref="NetworkPackage" /> to all specified receivers.
        /// </summary>
        /// <param name="pkg">
        ///     <see cref="NetworkPackage" /> to send.
        /// </param>
        public void Send(INetworkPackage pkg)
        {
            if (_client == null)
            {
                // Do NOT call AciLog here to prevent looped method call
                Debug.unityLogger.LogError(GetType().Name,"Not properly initialized. Please call the Initialize() method before sending data.");
                return;
            }

            string json = JsonConvert.SerializeObject(pkg);
            // Do NOT call AciLog here to prevent looped method call
            Debug.unityLogger.Log(GetType().Name, "Sending package \"" + json + "\"");
            try
            {
                // Convert to binary
                byte[] data = Encoding.UTF8.GetBytes(json);
                for (int i = 0; i < _remoteEndPoints.Length; ++i)
                    // Send msg to receiver
                    _client.Send(data, data.Length, _remoteEndPoints[i]);
            }
            catch (Exception err)
            {
                AciLog.LogError(GetType().Name, err);
            }
        }


        /// <summary>
        ///     Sends a <see cref="NetworkPackage" /> to a specific receiver.
        /// </summary>
        /// <param name="pkg">
        ///     <see cref="NetworkPackage" /> to send.
        /// </param>
        /// <param name="receiver">IP Endpoint as defined by the index of ip and port.</param>
        public void Send(INetworkPackage pkg, int receiver)
        {
            if (_client == null)
            {
                // Do NOT call AciLog here to prevent looped method call
                Debug.unityLogger.LogError(GetType().Name, "Not properly initialized. Please call the Initialize() method before sending data.");
                return;
            }

            if (receiver <= _remoteEndPoints.Length)
            {
                // Do NOT call AciLog here to prevent looped method call
                Debug.unityLogger.LogError(GetType().Name, "Target IP Endpoint at index " + receiver + " does not exist");
                return;
            }

            string json = JsonConvert.SerializeObject(pkg);
            // Do NOT call AciLog here to prevent looped method call
            Debug.unityLogger.Log(GetType().Name, "Sending package \"" + json + "\"");
            try
            {
                // Convert to binary
                byte[] data = Encoding.UTF8.GetBytes(json);
                // Send msg to receiver
                _client.Send(data, data.Length, _remoteEndPoints[receiver]);
            }
            catch (Exception err)
            {
                AciLog.LogError(GetType().Name, err);
            }
        }
    }
}