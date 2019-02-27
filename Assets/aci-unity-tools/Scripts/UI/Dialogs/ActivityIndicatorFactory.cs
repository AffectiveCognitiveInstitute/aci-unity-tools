using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.Dialog
{
    public class ActivityIndicatorFactory : MonoBehaviour, IActivityIndicatorFactory
    {
        [SerializeField]
        public GameObject m_Prefab;
        private DiContainer m_Container;

        [Zenject.Inject]
        private void Construct(DiContainer container)
        {
            m_Container = container;
        }

        public ActivityIndicatorDialog Create(string message)
        {
            var instance = m_Container.InstantiatePrefabForComponent<ActivityIndicatorDialog>(m_Prefab);
            instance.Initialize(message);
            return instance;
        }
    }
}

