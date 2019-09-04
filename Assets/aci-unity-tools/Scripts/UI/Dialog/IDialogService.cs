namespace Aci.Unity.UI.Dialog
{
    /// <summary>
    /// Gives access to sending dialog requests
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Sends a dialog request
        /// </summary>
        /// <param name="request">The request to be sent</param>
        void SendRequest(DialogRequest request);
    }
}