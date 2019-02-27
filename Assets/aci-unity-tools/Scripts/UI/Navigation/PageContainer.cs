using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Implementation of <see cref="IPageContainer"/> specifically for UGUI
    /// </summary>
    [RequireComponent(typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster))]
    public class PageContainer : MonoBehaviour, IPageContainer
    {
        [SerializeField]
        private GameObject m_PagePrefab;

        [SerializeField]
        private string m_PageId;

        private GameObject m_PageInstance;
        private IPageTransition m_PageTransition;
        private RectTransform m_Transform;
        private Canvas m_Canvas;
        private IPageContainerRegistry m_ContainerRegistry;

        [Inject]
        private IInstantiator m_Instantiator;

        private readonly List<INavigationEvent> m_NavigationEvents = new List<INavigationEvent>(10);

        /// <inheritdoc />
        public string Id
        {
            get
            {
                return m_PageId;
            }
        }

        protected void Awake()
        {
            m_Transform = GetComponent<RectTransform>();
            m_Canvas = GetComponent<Canvas>();
            if (string.IsNullOrEmpty(m_PageId))
                throw new ArgumentNullException("m_ScreenId is empty!");

            if (m_PagePrefab == null)
                throw new MissingReferenceException("You must provide a prefab");
        }

        [Inject]
        public void Construct(IPageContainerRegistry registry)
        {
            m_ContainerRegistry = registry;
            m_ContainerRegistry.Register(this);
        }

        /// <inheritdoc />
        public void Display(Action callback = null)
        {
            //m_PageInstance.SetActive(true);
            m_Canvas.enabled = true;

            if (callback != null)
                callback.Invoke();
        }

        /// <inheritdoc />
        public void Hide(string newPageId, Action callback = null)
        {
            //m_PageInstance.SetActive(false);
            m_Canvas.enabled = false;

            if (m_PageTransition != null)
                m_PageTransition.OnPagePopped(callback);
            else
            {
                if (callback != null)
                    callback.Invoke();
            }
        }

        /// <inheritdoc />
        public void OnNavigatedBack(INavigationParameters parameters)
        {
            m_NavigationEvents.Clear();
            m_PageInstance.GetComponentsInChildren(m_NavigationEvents);

            for (int i = 0; i < m_NavigationEvents.Count; i++)
                (m_NavigationEvents[i] as INavigatedBackAware)?.OnNavigatedBack(parameters);
        }

        /// <inheritdoc />
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            m_NavigationEvents.Clear();
            m_PageInstance.GetComponentsInChildren(m_NavigationEvents);

            for (int i = 0; i < m_NavigationEvents.Count; i++)
                (m_NavigationEvents[i] as INavigatedToAware)?.OnNavigatedTo(parameters);
        }

        /// <inheritdoc />
        public void OnNavigatingTo(INavigationParameters parameters)
        {
            m_NavigationEvents.Clear();
            m_PageInstance.GetComponentsInChildren(m_NavigationEvents);

            for (int i = 0; i < m_NavigationEvents.Count; i++)
                (m_NavigationEvents[i] as INavigatingAware)?.OnNavigatingTo(parameters);
        }


        /// <inheritdoc/>
        public void OnNavigatedAway(INavigationParameters parameters)
        {
            m_NavigationEvents.Clear();
            m_PageInstance.GetComponentsInChildren(m_NavigationEvents);

            for (int i = 0; i < m_NavigationEvents.Count; i++)
                (m_NavigationEvents[i] as INavigatedAwayAware)?.OnNavigatedAway(parameters);
        }

        /// <inheritdoc />
        public void PrepareDisplay()
        {
            if(m_PageInstance == null)
            {

                m_PageInstance = m_Instantiator.InstantiatePrefab(m_PagePrefab);

                m_PageInstance.transform.SetParent(m_Transform, false);
                m_PageTransition = m_PageInstance.GetComponent<IPageTransition>();
                m_Canvas.enabled = false; // It's better to disable the canvas from a performance standpoint
            }
        }

        private void OnDestroy()
        {
            if(m_ContainerRegistry != null)
                m_ContainerRegistry.Unregister(m_PageId);
        }

        /// <inheritdoc />
        public void Destroy()
        {
            if (m_PageInstance != null)
            {
                Destroy(m_PageInstance);
            }
        }
    }
}
