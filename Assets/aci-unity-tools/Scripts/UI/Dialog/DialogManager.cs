using Aci.Collections;
using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    public class DialogManager : MonoBehaviour, 
                                 IDialogRequestReceiver,
                                 IDialogDestroyer
    {
        private MaxHeap<DialogRequest> m_Requests = new MaxHeap<DialogRequest>(10);
        private DialogRequest m_Current;
        private Transform m_Transform;

        private void Awake()
        {
            m_Transform = GetComponent<Transform>();
        }

        private void OnDestroy()
        {
            Clear();
        }

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

        public void DestroyCurrent(bool animate = true)
        {
            if (m_Current != null)
                m_Current.dialog.Dismiss(animate);
        }
    }
}