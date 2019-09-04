using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    public abstract class NavigationCommandComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("When should navigation command be executed?")]
        private NavigationTrigger m_NavigationTrigger;

        [SerializeField, Tooltip("The animation type: None = No animation.\nSynchronous = Both screens will play" +
            "their animations simultaneously.\nAsynchronous = Screens will play their animations one after the other.")]
        protected AnimationMode m_AnimationMode = AnimationMode.Asynchronous;

        protected INavigationService m_NavigationService;

        [Zenject.Inject]
        private void Construct(INavigationService navigationService)
        {
            m_NavigationService = navigationService;
        }

        protected virtual void Awake()
        {
            if (m_NavigationTrigger == NavigationTrigger.OnAwake)
                Execute();
        }

        protected virtual void Start()
        {
            if (m_NavigationTrigger == NavigationTrigger.OnStart)
                Execute();
        }

        protected virtual void OnEnable()
        {
            if (m_NavigationTrigger == NavigationTrigger.OnEnable)
                Execute();
        }

        protected virtual void OnDisable()
        {
            if (m_NavigationTrigger == NavigationTrigger.OnDisable)
                Execute();
        }

        protected virtual void OnDestroy()
        {
            if (m_NavigationTrigger == NavigationTrigger.OnDestroy)
                Execute();
        }

        public abstract void Execute();
    }
}
