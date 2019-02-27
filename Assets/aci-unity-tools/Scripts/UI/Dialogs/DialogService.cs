using System.Collections.Generic;
using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    public class DialogService : MonoBehaviour, IDialogService
    {
        [SerializeField]
        private Transform m_Transform;

        private IDialog m_CurrentDialog;

        private readonly List<IDialog> m_Dialogs = new List<IDialog>(10);

        public void ShowDialog(IDialog dialog)
        {
            dialog.Dismissed += OnDialogDismissed;
            var t = dialog.gameObject.transform;
            t.SetParent(m_Transform, false);
            t.SetAsLastSibling();
            m_Dialogs.Add(dialog);
        }

        private void OnDialogDismissed(IDialog dialog)
        {
            m_Dialogs.Remove(dialog);
        }

        public void ClearAll()
        {
            m_Dialogs.ForEach(d => d.Dismiss());
            m_Dialogs.Clear();
        }
    }
}

