using Aci.UI.Collections;
using System;

namespace Aci.Unity.UI.Dialog
{
    public class DialogRequest : IComparable<DialogRequest>, IDisposable
    {
        private static Pool<DialogRequest> m_RequestPool;

        public int priority { get; private set; }
        public IDialog dialog { get; private set; }
        public event Action<DialogRequest> disposed;

        static DialogRequest()
        {
            m_RequestPool = new Pool<DialogRequest>(() =>
            {
                return new DialogRequest();
            });
        }

        protected DialogRequest()
        {
            this.dialog = null;
            priority = -1;
        }

        ~DialogRequest()
        {
            Dispose();
        }

        public static DialogRequest Create(IDialog dialog, int priority)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));
            
            var request = m_RequestPool.Get();
            request.dialog = dialog;
            request.priority = priority;
            dialog.dismissed += request.OnDialogDismissed;
            return request;
        }

        private void OnDialogDismissed(IDialog dialog)
        {
            Dispose();
        }

        public static DialogRequest Create(IDialog dialog, DialogPriority priority = DialogPriority.Medium)
        {
            return Create(dialog, (int)priority);
        }

        public void Dispose()
        {
            if (dialog != null)
            {
                dialog.dismissed -= OnDialogDismissed;
            }

            dialog = null;
            priority = -1;
            m_RequestPool.Return(this);
            disposed?.Invoke(this);
        }

        public int CompareTo(DialogRequest other)
        {
            return priority.CompareTo(other.priority);
        }
    }
}