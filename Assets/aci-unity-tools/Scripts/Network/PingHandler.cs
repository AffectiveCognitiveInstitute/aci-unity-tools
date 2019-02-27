using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Zenject;

namespace Aci.Unity.Network
{
    public sealed class PingPackage : INetworkPackage
    {
        public const string CallType = "ping";

        [JsonProperty("call")]
        public string call { get; } = CallType;

        [JsonProperty("id")]
        public int id;
    }

    public class PingHandler : NetworkPackageHandlerBase<PingPackage>
    {
        private INetworkPublisher m_Publisher;
        private float[] m_PendingPings;

        [Inject]
        public void Construct(INetworkPublisher publisher, NetworkSubscriber subscriber)
        {
            m_Publisher = publisher;
        }

        /// <summary>
        ///     Tries to ping a network client.
        /// </summary>
        /// <param name="id">Target client's id as referenced in <see cref="NetworkPublisher"/> instance.</param>
        /// <returns>True if answer was received, False otherwise.</returns>
        public async Task<bool> WaitForPing(int id, int maxWait, float startTime, float maxDelta)
        {
            if (id >= m_PendingPings.Length)
                Array.Resize(ref m_PendingPings, id-1);
            m_PendingPings[id] = 0;
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.CancelAfter(maxWait * 1000);
            m_Publisher.Send(new PingPackage(){id = id}, id);

            try
            {
                await awaitPing(id, startTime, maxDelta, cancelTokenSource.Token);
                return true;
            }
            catch (OperationCanceledException c)
            {
                //timeout, return false
                return false;
            }
        }

        /// <inheritdoc />
        public override void Handle(PingPackage package)
        {
            m_PendingPings[package.id] = Time.time;
        }

        private async Task awaitPing(int id, float startTime, float maxDelta, CancellationToken token)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                token.ThrowIfCancellationRequested();
                if (Math.Abs(startTime + (float)watch.Elapsed.TotalSeconds - m_PendingPings[id]) <= maxDelta)
                    return;
            }
        }
    }

}
