using System;

namespace Aci.Unity.UI.Dialog
{
    public interface IAlertDialogFactory 
    {
        AlertDialog Create(string title, string message, string cancel, string confirm, Action onConfirm = null, Action onCancel = null);
    }
}

