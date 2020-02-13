using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Aci.Unity.UI.Dialog
{
    /// <summary>
    ///     Add this component to GameObjects that represent a Dialog or Popup.
    /// </summary>
    public class DialogComponent : MonoBehaviour, IDialog
    {
        [System.Serializable]
        public class DialogAppearedEvent : UnityEvent<DialogComponent> { }

        private ITransition m_Transition;
        private bool m_IsBusy = false;

        [SerializeField]
        private DialogAppearedEvent m_DialogAppeared;

        public DialogAppearedEvent dialogAppeared => m_DialogAppeared;

        /// <inheritdoc />
        public event DialogDismissedDelegate dismissed;

        private void Awake()
        {
            m_Transition = GetComponent<ITransition>();
        }

        /// <inheritdoc />
        public async void Dismiss(bool animated = true)
        {
            if (m_IsBusy)
                return;

            m_IsBusy = true;

            try
            {
                if (!animated || m_Transition == null || !gameObject.activeSelf)
                {
                    dismissed?.Invoke(this);
                }
                else
                {
                    if (m_Transition != null)
                    {
                        try
                        {
                            await m_Transition.ExitAsync();
                            dismissed?.Invoke(this);
                        }
                        catch (System.Exception e)
                        {
#if UNITY_EDITOR
                            if (!UnityEditor.EditorApplication.isPlaying)
                                return;
#else
                        Debug.LogException(e);
#endif
                        }

                    }
                }

                await Task.Delay(200);
            }
            finally
            {
                m_IsBusy = false;
            }
        }

        /// <inheritdoc />
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <inheritdoc />
        public async void Show(bool animated = true)
        {
            gameObject.SetActive(true);

            if (animated && m_Transition != null)
                await m_Transition.EnterAsync();

            dialogAppeared?.Invoke(this);
        }
    }
}
