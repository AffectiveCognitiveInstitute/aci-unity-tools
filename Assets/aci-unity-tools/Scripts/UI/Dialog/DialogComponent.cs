using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    public class DialogComponent : MonoBehaviour, IDialog
    {
        private ITransition m_Transition;
        private bool m_IsDestroyed;
        private GameObject m_CachedGameObject;

        /// <inheritdoc />
        public event DialogDismissedDelegate dismissed;

        private void Awake()
        {
            m_CachedGameObject = gameObject;
            m_Transition = GetComponent<ITransition>();
        }

        /// <inheritdoc />
        public async void Dismiss(bool animated = true)
        {
            if (m_IsDestroyed)
                return;

            if (!animated || m_Transition == null || !gameObject.activeSelf)
            {
                Destroy(gameObject);
            }
            else
            {
                if(m_Transition != null)
                {
                    try
                    {
                        await m_Transition.ExitAsync();
                        
                        object goAsObj = m_CachedGameObject;

                        if(goAsObj != null && m_CachedGameObject != null)
                            Destroy(m_CachedGameObject);
                    }
                    catch(System.Exception e)
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
        }

        private void OnApplicationQuit()
        {
            m_IsDestroyed = true;
        }

        private void OnDestroy()
        {
            dismissed?.Invoke(this);
            m_IsDestroyed = true;
        }
    }
}
