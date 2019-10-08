// <copyright file=ITimeProvider.cs/>
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
// <date>08/16/2018 15:48</date>

using System;
using UnityEngine;

namespace Aci.Unity.Util
{
    /// <summary>
    /// Interface for time tracking class.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Time this tracker started tracking.
        /// </summary>
        DateTime startTime { get; }

        /// <summary>
        /// Time this tracker started tracking the current subTime.
        /// </summary>
        DateTime subStartTime { get; }

        /// <summary>
        /// Elapsed Time since starting this tracker.
        /// </summary>
        TimeSpan elapsed { get; }

        /// <summary>
        /// Elapsed Time since starting this tracker.
        /// </summary>
        TimeSpan elapsedTotal { get; }

        /// <summary>
        /// Pause indicator. True if paused, false otherwise. Pauses the tracker if set true, resumes it if set false.
        /// </summary>
        bool paused { get; set; }

        /// <summary>
        /// Resets the timer start time.
        /// </summary>
        /// <param name="total">False if only elapsed time should be reset, True if total time should be reset.</param>
        void Reset(bool total = false);

        /// <summary>
        /// Adds a timespan to the current timer value.
        /// </summary>
        /// <param name="timeSpan">Timespawn to add.</param>
        /// <param name="total">False if only elapsed time should be added, True if total time should be added.</param>
        void Add(TimeSpan timeSpan, bool total = false);
    }
}
