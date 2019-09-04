using System;

namespace Aci.Unity.UI.Dialog
{
    /// <summary>
    /// Interface which exposes commonly used dialogs
    /// </summary>
    public interface IDialogFacade
    {
        /// <summary>
        ///     Creates an alert dialog made up of Title, Message, Cancel button, Confirm button.
        /// </summary>
        /// <param name="title">The title of this dialog</param>
        /// <param name="message">The message of this dialog</param>
        /// <param name="cancel">The text on the Cancel button</param>
        /// <param name="confirm">The text on the Confirm button</param>
        /// <param name="onConfirm">A callback when the confirm button is pressed</param>
        /// <param name="onCancel">A callback when the cancel button is pressed</param>
        /// <returns>Returns an instance of <see cref="IDialog"/></returns>
        IDialog DisplayAlert(string title, string message, string cancel, string confirm, Action onConfirm = null, Action onCancel = null);

        /// <summary>
        ///     Creates an alert dialog made up of Title, Message, Cancel button, Confirm button.
        /// </summary>
        /// <param name="priority">The priority of the dialog. Higher priorities are displayed first.</param>
        /// <param name="title">The title of this dialog</param>
        /// <param name="message">The message of this dialog</param>
        /// <param name="cancel">The text on the Cancel button</param>
        /// <param name="confirm">The text on the Confirm button</param>
        /// <param name="onConfirm">A callback when the confirm button is pressed</param>
        /// <param name="onCancel">A callback when the cancel button is pressed</param>
        /// <returns>Returns an instance of <see cref="IDialog"/></returns>
        IDialog DisplayAlert(DialogPriority priority, string title, string message, string cancel, string confirm, Action onConfirm = null, Action onCancel = null);

        /// <summary>
        ///     Displays an activity indicator dialog
        /// </summary>
        /// <param name="message">The message to be displayed on the dialog</param>
        /// <returns>Returns an instance of <see cref="IDialog"/></returns>
        IDialog DisplayActivity(string message);

        /// <summary>
        ///     Displays an activity indicator dialog
        /// </summary>
        /// <param name="priority">The priority of the dialog. Higher priorities are displayed first.</param>
        /// <param name="message">The message to be displayed on the dialog</param>
        /// <returns>Returns an instance of <see cref="IDialog"/></returns>
        IDialog DisplayActivity(DialogPriority priority, string message);
    }
}
