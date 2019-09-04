using Aci.Collections;
using System;

namespace Aci.Unity.UI.Dialog
{
    /// <summary>
    ///     Manages instances of <see cref="DialogRequest"/>.
    /// </summary>
    public class DialogManager : IDialogRequestReceiver,
                                 IDialogDestroyer,
                                 IDisposable
    {
        // Sorts dialog requests by their priority.
        private readonly MaxHeap<DialogRequest> m_Requests = new MaxHeap<DialogRequest>(10);

        private DialogRequest m_Current; // The current request being processed.

        /// <inheritdoc />
        public void ReceiveRequest(DialogRequest request)
        {
            request.disposed += OnRequestDisposed;

            if (m_Current == null)
            {
                m_Current = request;
                m_Current.dialog.Show();
            }
            else
            {
                if(m_Current.priority < request.priority)
                {
                    m_Current.dialog.Hide();
                    m_Requests.Push(m_Current);
                    m_Current = request;
                    m_Current.dialog.Show();
                }
                else
                {
                    m_Requests.Push(request);
                }
            }
        }

        private void OnRequestDisposed(DialogRequest request)
        {
            request.disposed -= OnRequestDisposed;

            if(m_Current == request)
            {
                m_Current = null;
                DisplayNext();
            }
            else
            {
                m_Requests.Remove(request);
            }
        }

        private void DisplayNext()
        {
            if (m_Requests.Count == 0)
                return;

            m_Current = m_Requests.Pop();
            m_Current.dialog.Show();
        }

        /// <inheritdoc />
        public void Clear()
        {
            if(m_Current != null)
            {
                m_Current.disposed -= OnRequestDisposed;
                m_Current.dialog.Dismiss(false);
            }

            foreach(var request in m_Requests)
            {
                if (request == null)
                    continue;

                request.disposed -= OnRequestDisposed;
                request.dialog?.Dismiss(false);
            }
        }

        /// <inheritdoc />
        public void DestroyCurrent(bool animate = true)
        {
            if (m_Current != null)
                m_Current.dialog.Dismiss(animate);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Clear();
        }
    }
}