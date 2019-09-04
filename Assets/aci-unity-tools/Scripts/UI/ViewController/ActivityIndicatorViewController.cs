using Aci.Unity.UI.Dialog;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.ViewControllers
{
    [RequireComponent(typeof(DialogComponent))]
    public class ActivityIndicatorViewController : MonoBehaviour, IPoolable<string, IMemoryPool>, IDisposable
    {
        [SerializeField]
        private TextMeshProUGUI m_Message;
        private IMemoryPool m_Pool;
        private DialogComponent m_Dialog;

        private void Awake()
        {
            m_Dialog = GetComponent<DialogComponent>();
        }

        private void OnEnable()
        {
            m_Dialog.dismissed += OnDialogDismissed;
        }

        private void OnDisable()
        {
            m_Dialog.dismissed -= OnDialogDismissed;
        }

        private void OnDialogDismissed(IDialog dialog)
        {
            Dispose();
        }

        public void Initialize(string message)
        {
            m_Message.text = message;
        }

        public void Dispose()
        {
            m_Pool.Despawn(this);
        }

        public void OnDespawned()
        {
            m_Pool = null;
        }

        public void OnSpawned(string text, IMemoryPool pool)
        {
            m_Pool = pool;
            Initialize(text);
        }

        public static implicit operator DialogComponent(ActivityIndicatorViewController vc)
        {
            return vc.m_Dialog;
        }

        public class Factory : PlaceholderFactory<string, ActivityIndicatorViewController> { }
    }
}