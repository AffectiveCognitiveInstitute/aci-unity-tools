using System;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.Dialog
{
    public class AlertDialogFactory : MonoBehaviour, IAlertDialogFactory
    {
        [SerializeField]
        private GameObject m_Prefab;
        private DiContainer m_Container;

        [Zenject.Inject]
        private void Construct(DiContainer container)
        {
            m_Container = container;
        }

        public AlertDialog Create(string title, string message, string cancel, string confirm, Action onConfirm = null, Action onCancel = null)
        {
            var instance = m_Container.InstantiatePrefabForComponent<AlertDialog>(m_Prefab);
            instance.Initialize(title, message, cancel, confirm, onCancel, onConfirm);
            return instance;
        }
    }
}

