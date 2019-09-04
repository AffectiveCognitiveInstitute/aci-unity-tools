using System;

namespace Aci.Unity.UI.Dialog
{
    public class DialogService : IDialogService
    {
        private readonly IDialogRequestReceiver m_RequestReceiver;

        public DialogService(IDialogRequestReceiver requestReceiver)
        {
            if (requestReceiver == null)
                throw new ArgumentNullException(nameof(requestReceiver));

            m_RequestReceiver = requestReceiver;
        }

        public void SendRequest(DialogRequest request)
        {
            m_RequestReceiver.ReceiveRequest(request);
        }
    }
}
