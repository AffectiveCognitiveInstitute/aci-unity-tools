using System;
using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    public class AlertDialog : DialogBase
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI m_Title;
        [SerializeField]
        private TMPro.TextMeshProUGUI m_Message;
        [SerializeField]
        private TMPro.TextMeshProUGUI m_Cancel;
        [SerializeField]
        private TMPro.TextMeshProUGUI m_Confirm;

        [SerializeField]
        private GameObject m_ConfirmButton;
        [SerializeField]
        private GameObject m_CancelButton;

        private Action m_OnCancel;
        private Action m_OnConfirm;

        public void Initialize(string title,
                               string message,
                               string cancel,
                               string confirm,
                               Action onCancel,
                               Action onConfirm)
        {
            SetString(m_Title, title);
            SetString(m_Message, message);
            SetString(m_Cancel, cancel);
            SetString(m_Confirm, confirm);

            m_OnCancel = onCancel;
            m_OnConfirm = onConfirm;

            m_CancelButton.SetActive(!string.IsNullOrEmpty(cancel));
            m_ConfirmButton.SetActive(!string.IsNullOrEmpty(confirm));
        }

        private void SetString(TMPro.TextMeshProUGUI text, string s)
        {
            if (string.IsNullOrEmpty(s))
                text.gameObject.SetActive(false);
            else
                text.text = s;
        }

        public void OnCancel()
        {
            Dismiss();
            m_OnCancel?.Invoke();
        }

        public void OnConfirm()
        {
            Dismiss();
            m_OnConfirm?.Invoke();
        }
    }  
}