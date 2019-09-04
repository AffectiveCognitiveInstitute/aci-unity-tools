namespace Aci.Unity.UI.Dialog
{
    public interface IDialogDestroyer
    {
        /// <summary>
        ///     Destroys all dialogs.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Destroys the current dialog.
        /// </summary>
        void DestroyCurrent(bool animate = true);
    }
}
