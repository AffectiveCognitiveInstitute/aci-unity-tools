// <copyright file=TweenerDirector.cs/>
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
//   James Gay, Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>08/01/2018 06:16</date>

using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Aci.Unity.UI.Tweening
{
    /// <summary>
    ///     A class which is responsible for executing multiple tweeners
    /// </summary>
    public class TweenerDirector : MonoBehaviour
    {
        public enum PlayTweenEventType
        {
            Awake,
            Start,
            OnEnable,
            Custom
        }

        [SerializeField] private PlayTweenEventType m_EventType;

        [SerializeField] private Tweener[] m_Tweeners;

        private float m_Value = 0f;

        public Tweener[] tweeners => m_Tweeners;

        public float Value
        {
            get => m_Value;
            set => Seek(m_Value);
        }

        private void Awake()
        {
            if (m_EventType == PlayTweenEventType.Awake)
                PlayForwards();
        }

        private void Start()
        {
            if (m_EventType == PlayTweenEventType.Start)
                PlayForwards();
        }

        private void OnEnable()
        {
            if (m_EventType == PlayTweenEventType.OnEnable)
                PlayForwards();
        }

        /// <summary>
        /// Adds a Tweener at the end of the tweener array.
        /// </summary>
        /// <param name="tweener">Target tweener to add.</param>
        public void AddTweener(Tweener tweener)
        {
            Array.Resize(ref m_Tweeners, m_Tweeners.Length+1);
            m_Tweeners[m_Tweeners.Length - 1] = tweener;
        }

        /// <summary>
        /// Removes all tweeners after target index. Resizes tweener array.
        /// </summary>
        /// <param name="index">Target index.</param>
        public void RemoveTweenersAfter(int index)
        {
            Array.Resize(ref m_Tweeners, index + 1);
        }

        public void Seek(float timeNormalized)
        {
            m_Value = timeNormalized;

            if (m_Tweeners == null)
                return;

            int count = m_Tweeners.Length;
            float totalDuration = GetTotalDuration();
            float realtime = Mathf.Lerp(0f, totalDuration, timeNormalized);
            for (int i = 0; i < count; i++)
            {
                float relativeNormalizedTime = Mathf.InverseLerp(m_Tweeners[i].delayTime, m_Tweeners[i].duration, realtime);
                m_Tweeners[i].Seek(relativeNormalizedTime);
            }
        }

        private float GetTotalDuration()
        {
            float max = 0f;
            for(int i = 0; i < m_Tweeners.Length; i++)
            {
                float duration = m_Tweeners[i].delayTime + m_Tweeners[i].duration;
                max = Mathf.Max(max, duration);
            }

            return max;
        }

        [ContextMenu("Play forwards")]
        public void PlayForwards(bool triggerEvents = true)
        {
            int count = m_Tweeners.Length;
            for (int i = 0; i < count; i++)
                m_Tweeners[i].PlayForwards(triggerEvents);
        }

        public async Task PlayForwardsAsync(bool triggerEvents = true)
        {
            Task[] tasks = new Task[m_Tweeners.Length];
            for (int i = 0; i < m_Tweeners.Length; i++)
                tasks[i] = m_Tweeners[i].PlayForwardsAsync(triggerEvents);

            await Task.WhenAll(tasks);
        }

        [ContextMenu("Play reverse")]
        public void PlayReverse(bool triggerEvents = true)
        {
            int count = m_Tweeners.Length;
            for (int i = 0; i < count; i++)
                m_Tweeners[i].PlayReverse(triggerEvents);
        }

        public async Task PlayReverseAsync(bool triggerEvents = true)
        {
            Task[] tasks = new Task[m_Tweeners.Length];
            for (int i = 0; i < m_Tweeners.Length; i++)
                tasks[i] = m_Tweeners[i].PlayReverseAsync(triggerEvents);

            await Task.WhenAll(tasks);
        }

        [ContextMenu("Play toggled")]
        public void PlayToggled(bool triggerEvents = true)
        {
            int count = m_Tweeners.Length;
            for (int i = 0; i < count; i++)
                m_Tweeners[i].PlayToggled(triggerEvents);
        }

        public async Task PlayToggledAsync(bool triggerEvents = true)
        {
            Task[] tasks = new Task[m_Tweeners.Length];
            for (int i = 0; i < m_Tweeners.Length; i++)
                tasks[i] = m_Tweeners[i].PlayToggledAsync(triggerEvents);

            await Task.WhenAll(tasks);
        }
    }
}