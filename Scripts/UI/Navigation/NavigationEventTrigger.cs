using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Aci.Unity.UI.Navigation;

namespace Aci.Unity.UI
{
    public class NavigationEventTrigger : MonoBehaviour, INavigationAware
    {
        public enum EventType
        {
            NavigatedBack,
            NavigatedTo,
            NavigatingAway,
            NavigatingBack,
            NavigatingTo,
            ScreenDestroyed
        }

        [System.Serializable]
        public class TriggerEvent : UnityEvent<INavigationParameters> { }

        [System.Serializable]
        public class Entry
        {
            /// <summary>
            ///     <para>The desired navigation event the callback is listening to.</para>
            /// </summary>
            public EventType eventType = EventType.NavigatedBack;

            /// <summary>
            ///     <para>The desired UnityEvent to be invoked.</para>
            /// </summary>
            public TriggerEvent navigationEvent = new TriggerEvent();
        }

        [SerializeField]
        private List<Entry> m_Delegates = new List<Entry>();

        public List<Entry> triggers => m_Delegates;

        private void Execute(EventType eventType, INavigationParameters navigationParameters)
        {
            for (int i = 0; i < m_Delegates.Count; i++)
            {
                if (m_Delegates[i].eventType == eventType)
                {
                    m_Delegates[i].navigationEvent?.Invoke(navigationParameters);
                    return;
                }
            }
        }

        public void OnNavigatedBack(INavigationParameters navigationParameters)
        {
            Execute(EventType.NavigatedBack, navigationParameters);
        }

        public void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            Execute(EventType.NavigatedTo, navigationParameters);
        }

        public void OnNavigatingAway(INavigationParameters navigationParameters)
        {
            Execute(EventType.NavigatingAway, navigationParameters);
        }

        public void OnNavigatingBack(INavigationParameters navigationParameters)
        {
            Execute(EventType.NavigatingBack, navigationParameters);
        }

        public void OnNavigatingTo(INavigationParameters navigationParameters)
        {
            Execute(EventType.NavigatingTo, navigationParameters);
        }

        public void OnScreenDestroyed(INavigationParameters navigationParameters)
        {
            Execute(EventType.ScreenDestroyed, navigationParameters);
        }
    }
}