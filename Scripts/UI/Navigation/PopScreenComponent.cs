using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Attach to GameObject with NGUI Button Script. Pops current screen from navigation stack on Button click.
    /// </summary>
    public class PopScreenComponent : NavigationCommandComponent
    {
        private bool m_IsBusy;

        override public async void Execute()
        {
            if (m_IsBusy)
                return;

            if (!m_NavigationService.CanNavigate())
                return;

            try
            {
                m_IsBusy = true;

                switch(m_AnimationMode)
                {
                    case AnimationMode.Off:
                        await m_NavigationService.PopAsync(AnimationOptions.None);
                        break;
                    case AnimationMode.Synchronous:
                        await m_NavigationService.PopAsync(AnimationOptions.Synchronous);
                        break;
                    case AnimationMode.Asynchronous:
                        await m_NavigationService.PopAsync(AnimationOptions.Asynchronous);
                        break;
                }
            }
            catch (System.Exception e)
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
