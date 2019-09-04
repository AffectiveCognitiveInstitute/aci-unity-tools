using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    /// <summary>
    ///     Delegate for when an <see cref="IDialog"/> was dismissed.
    /// </summary>
    /// <param name="dialog">The dialog dismissed.</param>
    public delegate void DialogDismissedDelegate(IDialog dialog); 

    /// <summary>
    ///     Interface for dialogs/popups.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        ///     Dismisses the dialog.
        /// </summary>
        /// <param name="animated">Should its dismissal be animated?</param>
        void Dismiss(bool animated = true);

        /// <summary>
        ///     Displays the dialog.
        /// </summary>
        /// <param name="animated">Should it be animated?</param>
        void Show(bool animated = true);

        /// <summary>
        ///     Hides the dialog.
        /// </summary>
        void Hide();

        /// <summary>
        ///     Invoked when the dialog was dismissed.
        /// </summary>
        event DialogDismissedDelegate dismissed;

        /// <summary>
        ///     The <see cref="GameObject"/> representing the dialog.
        /// </summary>
        GameObject gameObject { get; }
    }
}
