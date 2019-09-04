using Aci.Unity.UI.Dialog;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.ViewControllers
{
    [RequireComponent(typeof(DialogComponent))]
    public class AlertDialogViewController : MonoBehaviour,
                                             IPoolable<string, string, string, string, Action, Action, IMemoryPool>,
                                             IDisposable
    {
        [SerializeField]
        private TextMeshProUGUI m_Title;

        [SerializeField]
        private TextMeshProUGUI m_Description;

        [SerializeField]
        private TextMeshProUGUI m_CancelText;

        [SerializeField]
        private TextMeshProUGUI m_ConfirmText;

        [SerializeField]
        private GameObject m_ConfirmButton;

        [SerializeField]
        private GameObject m_CancelButton;

        private IMemoryPool m_Pool;
        private DialogComponent m_Dialog;
        private Action m_CancelAction;
        private Action m_ConfirmAction;

        private void Awake()
        {
            m_Dialog = GetComponent<DialogComponent>();
        }

        private void OnEnable()
        {
            m_Dialog.dismissed += OnDismissed;
        }

        private void OnDisable()
        {
            m_Dialog.dismissed -= OnDismissed;
        }

        private void OnDismissed(IDialog dialog)
        {
            Dispose();
        }

        public void Initialize(string title, string description, string cancel, string confirm, Action cancelAction, Action confirmAction)
        {
            SetText(m_Title, title);
            SetText(m_Description, description);
            SetText(m_CancelText, cancel);
            SetText(m_ConfirmText, confirm);

            m_CancelAction = cancelAction;
            m_ConfirmAction = confirmAction;

            m_ConfirmButton.SetActive(!string.IsNullOrWhiteSpace(confirm));
            m_CancelButton.SetActive(!string.IsNullOrWhiteSpace(cancel));
        }

        public void Dispose()
        {
            m_Pool.Despawn(this);
        }

        public void OnDespawned()
        {
            m_Pool = null;
        }

        public void OnSpawned(string title,
                              string description,
                              string cancel,
                              string confirm,
                              Action cancelAction,
                              Action confirmAction,
                              IMemoryPool pool)
        {
            m_Pool = pool;
            Initialize(title, description, cancel, confirm, cancelAction, confirmAction);
        }

        public void OnConfirmButtonClicked()
        {
            m_Dialog.Dismiss();
            m_ConfirmAction?.Invoke();
        }

        public void OnCancelButtonClicked()
        {
            m_Dialog.Dismiss();
            m_CancelAction?.Invoke();
        }

        private void SetText(TextMeshProUGUI label, string text)
        {
            label.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
            label.text = text;
        }

        public static implicit operator DialogComponent(AlertDialogViewController vc)
        {
            return vc.m_Dialog;
        }

        public class Factory : PlaceholderFactory<string, string, string, string, Action, Action, AlertDialogViewController> { }
    }
}
