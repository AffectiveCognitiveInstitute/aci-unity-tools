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

using Aci.Unity.UI.Navigation;
using System;
using UnityEngine;

namespace Aci.Unity.UI.Tweening
{
    /// <summary>
    ///     Triggers the execution of a <see cref="TweenerDirector"/> on a NavigationEvent
    /// </summary>
    public class TweenerDirectorNavigationTrigger : MonoBehaviour, INavigationAware
    {
        [SerializeField, Tooltip("The affected TweenerDirector")]
        private TweenerDirector m_TweenerDirector;

        [SerializeField, Tooltip("The navigation events the TweenerDirector will be executed")]
        private NavigationEvents m_Event = 0;

        [SerializeField, Tooltip("The playback type")]
        private PlaybackType m_PlaybackType = PlaybackType.Forwards;

        public enum PlaybackType
        {
            Forwards,
            Reverse,
            Toggle
        }

        [Flags]
        public enum NavigationEvents
        {
            NavigatedBack = 1 << 0,
            NavigatedTo = 1 << 1,
            NavigatingAway = 1 << 2,
            NavigatingBack = 1 << 3,
            NavigatingTo = 1 << 4,
            ScreenDestroyed = 1 << 5
        }


        private void Execute()
        {
            if (m_TweenerDirector == null)
                return;

            switch (m_PlaybackType)
            {
                case PlaybackType.Forwards:
                    m_TweenerDirector.PlayForwards();
                    break;
                case PlaybackType.Reverse:
                    m_TweenerDirector.PlayReverse();
                    break;
                case PlaybackType.Toggle:
                    m_TweenerDirector.PlayToggled();
                    break;
            }
        }

        public void OnNavigatedBack(INavigationParameters navigationParameters)
        {
            if (m_Event.HasFlag(NavigationEvents.NavigatedBack))
                Execute();
        }

        public void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            if (m_Event.HasFlag(NavigationEvents.NavigatedTo))
                Execute();
        }

        public void OnNavigatingAway(INavigationParameters navigationParameters)
        {
            if (m_Event.HasFlag(NavigationEvents.NavigatingAway))
                Execute();
        }

        public void OnNavigatingBack(INavigationParameters navigationParameters)
        {
            if (m_Event.HasFlag(NavigationEvents.NavigatingBack))
                Execute();
        }

        public void OnNavigatingTo(INavigationParameters navigationParameters)
        {
            if (m_Event.HasFlag(NavigationEvents.NavigatingTo))
                Execute();
        }

        public void OnScreenDestroyed(INavigationParameters navigationParameters)
        {
            if (m_Event.HasFlag(NavigationEvents.ScreenDestroyed))
                Execute();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            m_TweenerDirector = GetComponent<TweenerDirector>();
        }
#endif
    }
}