namespace Aci.Unity.UI.Dialog
{
    public interface IDialogRequestReceiver
    {
        /// <summary>
        /// Receives a dialog request
        /// </summary>
        /// <param name="request">The request to be received</param>
        void ReceiveRequest(DialogRequest request);
    }
}
