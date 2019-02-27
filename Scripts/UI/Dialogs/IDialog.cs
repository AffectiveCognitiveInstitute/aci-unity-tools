using System;
using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    /// <summary>
    /// Represents an interface for dialogs, e.g. Popups, Activity Indicators etc.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Event triggered when dialog is dismissed
        /// </summary>
        event Action<IDialog> Dismissed;

        /// <summary>
        /// Shows the dialog
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the dialog
        /// </summary>
        void Hide();

        /// <summary>
        /// Dismisses the dialog
        /// </summary>
        void Dismiss();

        /// <summary>
        /// A reference to the <see cref="GameObject"/>
        /// </summary>
        GameObject gameObject { get; }
    }
}

