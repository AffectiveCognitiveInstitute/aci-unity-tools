// <copyright file=AsyncTimeProvider.cs/>
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
// <date>08/16/2018 15:55</date>

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aci.Unity.Util
{
    /// <summary>
    /// Time Provider using async methods to track time over a certain timespan not goverened by frames.
    /// </summary>
    public class AsyncTimeProvider : ITimeProvider
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        private Stopwatch sw = new Stopwatch();

        private DateTime _startTime = DateTime.MinValue;
        /// <inheritdoc />
        public DateTime startTime => _startTime;

        private DateTime _subStartTime = DateTime.MinValue;

        /// <inheritdoc />
        public DateTime subStartTime => _subStartTime;

        private TimeSpan _elapsed = TimeSpan.Zero;
        /// <inheritdoc />
        public TimeSpan elapsed => _elapsed;

        private TimeSpan _elapsedTotal = TimeSpan.Zero;
        /// <inheritdoc />
        public TimeSpan elapsedTotal => _elapsedTotal;

        private bool _paused = true;
        /// <inheritdoc />
        public bool paused {
            get { return _paused; }
            set
            {
                if (value == _paused)
                    return;
                _paused = value;
                if (value)
                {
                    cts.Cancel();
                    sw.Start();
                    return;
                }

                if (!sw.IsRunning)
                {
                    _startTime = DateTime.Now;
                    _subStartTime = DateTime.Now;
                }
                sw.Stop();
                _startTime.Add(sw.Elapsed);
                _subStartTime.Add(sw.Elapsed);
                sw.Reset();
                cts =  new CancellationTokenSource();
                UpdateTrackedTime(cts.Token);
            }
        }

        /// <inheritdoc />
        public void Reset(bool total)
        {
            if (_paused && sw.IsRunning)
            {
                sw.Stop();
                sw.Reset();
            }
            _subStartTime = DateTime.Now;
            if (total)
                _startTime = DateTime.Now;
        }

        private async void UpdateTrackedTime(CancellationToken ct)
        {
            DateTime now;
            while (!ct.IsCancellationRequested)
            {
                now = DateTime.Now;
                _elapsedTotal = now.Subtract(_startTime);
                _elapsed = now.Subtract(_subStartTime);
                try
                {
                    await Task.Delay(40, ct);
                }
                catch(TaskCanceledException e) {
                    //this is fine
                }
            }
        }
    }
}