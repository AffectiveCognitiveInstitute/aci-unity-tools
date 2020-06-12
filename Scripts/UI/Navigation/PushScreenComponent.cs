using System;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Pushes target screen onto navigation stack.
    /// </summary>
    public class PushScreenComponent : NavigationCommandComponent
    {
        [SerializeField, Tooltip("The screen to navigate to.")]
        private string m_TargetScreen;
        
        [SerializeField, Tooltip("Should the current screen be added to navigation stack?")]
        private bool m_AddCurrentOntoNavigationStack = true;

        private bool m_IsBusy;

        override protected void Awake()
        {
            if (string.IsNullOrWhiteSpace(m_TargetScreen))
                throw new ArgumentNullException(m_TargetScreen, "Target screen may not be null or empty. Please enter a valid screen identifier!");

            base.Awake();
        }

        override public async void Execute()
        {
            if (m_IsBusy)
                return;

            if (!m_NavigationService.CanNavigate())
                return;

            m_IsBusy = true;

            try
            {
                switch (m_AnimationMode)
                {
                    case AnimationMode.Off:
                        await m_NavigationService.PushAsync(m_TargetScreen, AnimationOptions.None, m_AddCurrentOntoNavigationStack);
                        break;
                    case AnimationMode.Synchronous:
                        await m_NavigationService.PushAsync(m_TargetScreen, AnimationOptions.Synchronous, m_AddCurrentOntoNavigationStack);
                        break;
                    case AnimationMode.Asynchronous:
                        await m_NavigationService.PushAsync(m_TargetScreen, AnimationOptions.Asynchronous, m_AddCurrentOntoNavigationStack);
                        break;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                m_IsBusy = false;
            }
        }
    }
}
