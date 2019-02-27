using System;
using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    [RequireComponent(typeof(Canvas))]
    public class DialogBase : MonoBehaviour, IDialog
    {
        private Canvas m_Canvas;

        public event Action<IDialog> Dismissed;

        protected virtual void Awake()
        {
            m_Canvas = GetComponent<Canvas>();
        }

        public void Dismiss()
        {
            Destroy(gameObject);
        }

        public void Hide()
        {
            m_Canvas.enabled = false;
        }

        public void Show()
        {
            m_Canvas.enabled = true;
        }

        void OnDestroy()
        {
            Dismissed?.Invoke(this);
        }
    }
}

