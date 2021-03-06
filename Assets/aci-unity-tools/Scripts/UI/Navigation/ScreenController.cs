﻿using Aci.Unity.Events;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.Navigation
{
    [RequireComponent(typeof(Canvas))]
    [DefaultExecutionOrder(-100)]
    public class ScreenController : MonoBehaviour, IScreenController
    {
        public enum PrefabLoadingStrategy
        {
            FromReference,
            FromResource
        }

        [SerializeField]
        private string m_Id;

        [SerializeField]
        private PrefabLoadingStrategy m_PrefabLoadingStrategy;

        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private string m_PrefabPath;

        private GameObject m_Instance;
        private IScreenRegistry m_ScreenRegistry;
        private IAciEventManager m_EventManager;
        private IInstantiator m_Instantiator;
        private IScreenTransition m_ScreenTransition;
        private ICanNavigate[] m_CanNavigate;
        private Canvas m_Canvas;

        public string id => m_Id;

        [Inject]
        public void Construct(IScreenRegistry screenRegistry,
                              IInstantiator instantiator,
                              IAciEventManager eventManager)
        {
            m_ScreenRegistry = screenRegistry;
            m_EventManager = eventManager;
            m_Instantiator = instantiator;
        }

        private void Awake()
        {
            m_Canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            m_ScreenRegistry.Register(this);
        }

        private void OnDisable()
        {
            m_ScreenRegistry.Unregister(this);
        }

        public void Prepare()
        {
            if(m_Instance == null)
            {
                m_Instance = InstantiateScreen();
                m_ScreenTransition = m_Instance.GetComponent<IScreenTransition>();
                m_CanNavigate = m_Instance.GetComponentsInChildren<ICanNavigate>();
                m_Instance.transform.SetParent(transform, false);
                m_Instance.name = m_Id;
                m_Canvas.enabled = false;
            }
        }

        private GameObject InstantiateScreen()
        {
            if (m_PrefabLoadingStrategy == PrefabLoadingStrategy.FromResource)
                m_Prefab = Resources.Load<GameObject>(m_PrefabPath);

            return m_Instantiator.InstantiatePrefab(m_Prefab);
        }

        public bool Equals(IScreenController other)
        {
            return other.id == id;
        }

        public void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            if (ReferenceEquals(m_Instance, null) || m_Instance == null)
                return;

            ExecuteNavigationEvent.Execute(m_Instance, ExecuteNavigationEvent.navigatedTo, navigationParameters);
        }

        public void OnNavigatingAway(INavigationParameters navigationParameters)
        {
            if (ReferenceEquals(m_Instance, null) || m_Instance == null)
                return;

            ExecuteNavigationEvent.Execute(m_Instance, ExecuteNavigationEvent.navigatingAway, navigationParameters);
        }

        public void OnNavigatingBack(INavigationParameters navigationParameters)
        {
            if (ReferenceEquals(m_Instance, null) || m_Instance == null)
                return;

            ExecuteNavigationEvent.Execute(m_Instance, ExecuteNavigationEvent.navigatingBack, navigationParameters);
        }

        public void OnNavigatedBack(INavigationParameters navigationParameters)
        {
            if (ReferenceEquals(m_Instance, null) || m_Instance == null)
                return;

            ExecuteNavigationEvent.Execute(m_Instance, ExecuteNavigationEvent.navigatedBack, navigationParameters);
        }

        public void OnNavigatingTo(INavigationParameters navigationParameters)
        {
            if (ReferenceEquals(m_Instance, null) || m_Instance == null)
                return;

            ExecuteNavigationEvent.Execute(m_Instance, ExecuteNavigationEvent.navigatingTo, navigationParameters);
        }

        public void OnScreenDestroyed(INavigationParameters navigationParameters)
        {
            if (ReferenceEquals(m_Instance, null) || m_Instance == null)
                return;

            ExecuteNavigationEvent.Execute(m_Instance, ExecuteNavigationEvent.destroyed, navigationParameters);
        }

        public async Task UpdateDisplayAsync(NavigationMode navigationMode, bool animated = true)
        {
            Prepare();
            bool willAnimate = animated && m_ScreenTransition != null;

            m_EventManager.Invoke(new NavigationModeChangedEvent(this.m_Id, navigationMode));

            if(!willAnimate)
            {
                switch (navigationMode)
                {
                    case NavigationMode.Entering:
                    case NavigationMode.Returning:
                        m_Canvas.enabled = true;
                        if (m_ScreenTransition != null)
                            m_ScreenTransition.DisplayImmediately();
                        break;
                    case NavigationMode.Leaving:
                        m_Canvas.enabled = false;
                        break;
                    case NavigationMode.Removed:
                        Destroy(m_Instance);
                        m_Instance = null;
                        break;
                }
            }
            else
            {
                if (navigationMode == NavigationMode.Entering || navigationMode == NavigationMode.Returning)
                    m_Canvas.enabled = true;

                await m_ScreenTransition.MakeTransitionAsync(navigationMode);

                if (navigationMode == NavigationMode.Removed)
                    Destroy(m_Instance);
                else if (navigationMode == NavigationMode.Leaving)
                    m_Canvas.enabled = false;
            }
        }

        /// <inheritdoc/>
        public bool CanNavigate(INavigationParameters navigationParameters)
        {
            if (m_CanNavigate == null || m_CanNavigate.Length == 0)
                return true;

            for (int i = 0; i < m_CanNavigate.Length; i++)
            {
                if (!m_CanNavigate[i].CanNavigate(navigationParameters))
                    return false;
            }

            return true;
        }
    }
}
