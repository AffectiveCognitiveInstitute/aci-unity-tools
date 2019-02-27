namespace Aci.Unity.UI.Navigation
{
    public interface INavigatedBackAware : INavigationEvent
    {
        /// <summary>
        /// Called when returning to a page, i.e. after a page has been popped from the navigation stack and the animation
        /// has completed
        /// </summary>
        /// <param name="parameters">Parameters that have been passed over</param>
        void OnNavigatedBack(INavigationParameters parameters);
    }
}
