// <copyright file=Tweener.cs/>
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
// <date>07/27/2018 22:06</date>

using System;
using System.Collections;
using System.Threading.Tasks;
using Aci.Unity.Logging;
using UnityEngine;
using UnityEngine.Events;

namespace Aci.Unity.UI.Tweening
{
    public abstract class Tweener : MonoBehaviour
    {
        private Coroutine m_Coroutine;

        [SerializeField]
        private PlayMode m_PlayMode = PlayMode.Once;

        [SerializeField]
        private float m_DelayTime;

        [SerializeField]
        private float m_Duration = 1f;

        [SerializeField]
        private bool m_IgnoreTimeScale = true;

        [SerializeField]
        private bool m_PlayOnAwake;

        private float m_TimeElapsed;

        [SerializeField]
        private UnityEvent m_TweenEnded;

        [SerializeField]
        private UnityEvent m_TweenStarted;

        [SerializeField]
        private WaitForSeconds m_WaitForSecondsScaled;

        [SerializeField]
        private WaitForSecondsRealtime m_WaitForSecondsUnscaled;

        /// <summary>
        ///     The duration of the animation in seconds
        /// </summary>
        public float duration
        {
            get { return m_Duration; }
            set { m_Duration = value; }
        }

        /// <summary>
        ///     The amount of delay before playing the animation
        /// </summary>
        public float delayTime
        {
            get { return m_DelayTime; }
            set
            {
                m_DelayTime = Mathf.Max(0, value);
                if (m_IgnoreTimeScale)
                    m_WaitForSecondsUnscaled = new WaitForSecondsRealtime(m_DelayTime);
                else
                    m_WaitForSecondsScaled = new WaitForSeconds(m_DelayTime);
            }
        }

        /// <summary>
        ///     Gets the elapsed time on a tweener.
        ///     Can be used to check animation direction after animation has completed.
        /// </summary>
        public float elapsedTime => m_TimeElapsed;

        /// <summary>
        ///     Should time scale be ignored?
        /// </summary>
        public bool ignoreTimeScale
        {
            get { return m_IgnoreTimeScale; }
            set { m_IgnoreTimeScale = value; }
        }

        public float normalizedTime
        {
            get => Mathf.InverseLerp(m_DelayTime, m_Duration, m_TimeElapsed);
            set => Seek(Mathf.Clamp01(value));
        }

        /// <summary>
        ///     Event invoked when a tween has started
        /// </summary>
        public UnityEvent tweenStarted => m_TweenStarted;

        /// <summary>
        ///     Event invoked when a tween has ended
        /// </summary>
        public UnityEvent tweenEnded => m_TweenEnded;

        /// <summary>
        ///     Is the animation currently being played in the forward direction
        /// </summary>
        public bool isPlayingForwards { get; private set; } = true;

        /// <summary>
        ///     Is the animation currently being played
        /// </summary>
        public bool isPlaying { get; private set; }

        protected virtual void Awake()
        {
            //Seek(0f);
            if(m_DelayTime > 0)
            {
                m_WaitForSecondsScaled = new WaitForSeconds(m_DelayTime);
                m_WaitForSecondsUnscaled = new WaitForSecondsRealtime(m_DelayTime);
            }

            if (m_PlayOnAwake)
                Play(true);
        }

        private void Play(bool triggerEvents)
        {
            m_Coroutine = StartCoroutine(RunAnimation(triggerEvents));
        }

        private async Task PlayAsync(bool triggerEvents)
        {
            await RunAnimation(triggerEvents);
        }

        public void Seek(float timeNormalized)
        {
            m_TimeElapsed = Mathf.Lerp(0, m_Duration, timeNormalized);
            ExecuteFrame(timeNormalized);
        }

        public void ResetToBeginning()
        {
            m_TimeElapsed = 0;
            Seek(0);
        }

        public void ResetToEnd()
        {
            m_TimeElapsed = m_Duration;
            Seek(1f);
        }

        [ContextMenu("Play forwards")]
        /// <summary>
        /// Plays the tween forwards from the beginning.
        /// </summary>
        public void PlayForwards(bool triggerEvents = true)
        {
            if (isPlaying)
                Stop();

            m_TimeElapsed = 0f;
            isPlayingForwards = true;
            Play(triggerEvents);
        }

        /// <summary>
        /// Plays the tween forwards from the beginning.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        /// <returns>Returns an awaitable Task.</returns>
        public async Task PlayForwardsAsync(bool triggerEvents = true)
        {
            if (isPlaying)
                Stop();

            m_TimeElapsed = 0f;
            isPlayingForwards = true;
            await PlayAsync(triggerEvents);
        }

        /// <summary>
        /// Plays the tween forwards continuing from where it last stopped.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        public void PlayForwardsContinuous(bool triggerEvents = true)
        {
            if (m_PlayMode == PlayMode.Once)
                isPlayingForwards = true;

            if (!isPlaying)
                Play(triggerEvents);
        }

        /// <summary>
        /// Plays the tween forwards continuing from where it last stopped.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        /// <returns>Returns an awaitable Task.</returns>
        public Task PlayForwardsContinuousAsync(bool triggerEvents = true)
        {
            if (m_PlayMode == PlayMode.Once)
                isPlayingForwards = true;

            if (!isPlaying)
                return PlayAsync(triggerEvents);

            return Task.CompletedTask; // TODO: Ideally, this should return a cached task.
        }


        [ContextMenu("Play in reverse")]
        /// <summary>
        /// Plays the tween in reverse.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        public void PlayReverse(bool triggerEvents = true)
        {
            if (isPlaying)
                Stop();

            m_TimeElapsed = m_Duration;
            isPlayingForwards = false;
            Play(triggerEvents);
        }

        /// <summary>
        /// Plays the tween in reverse.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        /// <returns>Returns an awaitable Task.</returns>
        public async Task PlayReverseAsync(bool triggerEvents = true)
        {
            if (isPlaying)
                Stop();

            m_TimeElapsed = m_Duration;
            isPlayingForwards = false;
            await PlayAsync(triggerEvents);
        }

        /// <summary>
        /// Plays the tween in reverse continuing from the position the previous tween stopped.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        public void PlayReverseContinuous(bool triggerEvents = true)
        {
            if (m_PlayMode == PlayMode.Once)
                isPlayingForwards = false;

            if (!isPlaying)
                Play(triggerEvents);
        }

        /// <summary>
        /// Plays the tween in reverse continuing from the position the previous tween stopped.
        /// </summary>
        /// <param name="triggerEvents">Should events be triggered?</param>
        /// <returns>Returns an awaitable Task.</returns>
        public Task PlayReverseContinuousAsync(bool triggerEvents = true)
        {
            if (m_PlayMode == PlayMode.Once)
                isPlayingForwards = false;

            if (!isPlaying)
                return PlayAsync(triggerEvents);

            return Task.CompletedTask;
        }

        [ContextMenu("Toggle play")]
        /// <summary>
        /// Plays the animation in the opposite direction it was previously
        /// being played
        /// </summary>
        public void PlayToggled(bool triggerEvents = true)
        {
            isPlayingForwards = !isPlayingForwards;

            if (isPlaying)
                return;

            Play(triggerEvents);
        }

        public async Task PlayToggledAsync(bool triggerEvents = true)
        {
            isPlayingForwards = !isPlayingForwards;

            if (isPlaying)
                return;

            await PlayAsync(triggerEvents);
        }

        /// <summary>
        ///     Stops or pauses the animation if it is running
        /// </summary>
        public void Stop()
        {
            if (!isPlaying)
                return;

            try
            {
                if (m_Coroutine != null)
                    StopCoroutine(m_Coroutine);

                isPlaying = false;
            }
            catch (Exception e)
            {
                AciLog.LogException(e);
            }
        }

        private IEnumerator RunAnimation(bool triggerEvents)
        {
            // Wait for delay time if any
            if (m_DelayTime > 0f)
            {
                if (m_IgnoreTimeScale)
                    yield return m_WaitForSecondsUnscaled;
                else
                    yield return m_WaitForSecondsScaled;
            }

            float percentage = 0f;

            OnPreTween();
            isPlaying = true;

            // Fire event tween started
            if (m_TweenStarted != null && triggerEvents)
                m_TweenStarted.Invoke();

            // Run the bulk of the animation
            while (m_TimeElapsed >= 0 && m_TimeElapsed <= m_Duration)
            {
                // Update time depending on the type of play mode.
                UpdateTime(m_IgnoreTimeScale? Time.unscaledDeltaTime : Time.deltaTime);

                // Calculate how far along the animation is as a value between 0 and 1
                percentage = Mathf.Clamp01(m_TimeElapsed / m_Duration);

                // Execute animation logic for this frame
                ExecuteFrame(percentage);

                yield return null;
            }

            // Reset values
            m_TimeElapsed = Mathf.Clamp(m_TimeElapsed, 0, m_Duration);
            isPlaying = false;

            OnPostTween();

            // Fire event tween ended
            if (m_TweenEnded != null && triggerEvents)
                m_TweenEnded.Invoke();
        }

        private void UpdateTime(float time)
        {
            switch (m_PlayMode)
            {
                case PlayMode.Once:
                    if (isPlayingForwards)
                        m_TimeElapsed += time;
                    else
                        m_TimeElapsed -= time;
                    break;
                case PlayMode.Loop:
                    if(isPlayingForwards)
                    {
                        m_TimeElapsed += time;
                        if (m_TimeElapsed > m_Duration)
                            m_TimeElapsed -= m_Duration;
                    }
                    else
                    {
                        m_TimeElapsed -= time;
                        if (m_TimeElapsed < 0)
                            m_TimeElapsed += m_Duration;
                    }
                    break;
                case PlayMode.PingPong:
                    if(isPlayingForwards)
                    {
                        m_TimeElapsed += time;
                        if(m_TimeElapsed > m_Duration)
                        {
                            isPlayingForwards = false;
                            float delta = Mathf.Abs(m_Duration - m_TimeElapsed);
                            m_TimeElapsed = (m_Duration - delta);
                            Debug.Log(m_TimeElapsed);
                        }
                    }
                    else
                    {
                        m_TimeElapsed -= time;
                        if (m_TimeElapsed < 0)
                        {
                            isPlayingForwards = true;
                            m_TimeElapsed = Mathf.Abs(m_TimeElapsed);
                            //Debug.Log(m_TimeElapsed);
                        }
                    }
                    break;
            }            
        }

        protected abstract void ExecuteFrame(float percentage);

        protected virtual void OnPreTween()
        {
        }

        protected virtual void OnPostTween()
        {
        }

        protected virtual void Reset()
        {
        }

        protected virtual void OnValidate()
        {
            m_Duration = Mathf.Max(0, m_Duration);
            m_DelayTime = Mathf.Max(0, m_DelayTime);

            if (m_DelayTime > 0)
            {
                if (m_IgnoreTimeScale)
                    m_WaitForSecondsUnscaled = new WaitForSecondsRealtime(m_DelayTime);
                else
                    m_WaitForSecondsScaled = new WaitForSeconds(m_DelayTime);
            }
        }

        public enum PlayMode
        {
            Once,
            Loop,
            PingPong
        }
    }

    public abstract class Tweener<TValueType> : Tweener
    {
        [SerializeField] protected TValueType m_FromValue;

        [SerializeField] protected TValueType m_ToValue;

        [SerializeField] protected AnimationCurve m_Transition = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public TValueType fromValue
        {
            get { return m_FromValue; }
            set { m_FromValue = value; }
        }

        public TValueType toValue
        {
            get { return m_ToValue; }
            set { m_ToValue = value; }
        }

        public AnimationCurve transition
        {
            get { return m_Transition; }
            set { m_Transition = value; }
        }

        [ContextMenu("Set to start value")]
        protected virtual void SetToStartValue()
        {
            ExecuteFrame(0f);
        }

        [ContextMenu("Set to end value")]
        protected virtual void SetToEndValue()
        {
            ExecuteFrame(1f);
        }
    }

    public abstract class Tweener<TTargetType, TValueType> : Tweener<TValueType>
    {
        [SerializeField] protected TTargetType m_Target;

        public TTargetType target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        protected override void Awake()
        {
            if (m_Target == null)
                m_Target = GetComponent<TTargetType>();

            base.Awake();
        }

        protected override void Reset()
        {
            if (m_Target == null)
                m_Target = GetComponent<TTargetType>();
        }
    }
}
