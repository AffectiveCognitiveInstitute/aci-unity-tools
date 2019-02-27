namespace Aci.Unity.UI.Navigation
{
    public interface INavigatedToAware : INavigationEvent
    {
        /// <summary>
        /// Called when navigation to a page has completed, i.e. when a page has been pushed onto the navigation stack and the 
        /// animation has completed.
        /// </summary>
        /// <param name="parameters">Parameters passed to that page</param>
        void OnNavigatedTo(INavigationParameters parameters);
    }
}
