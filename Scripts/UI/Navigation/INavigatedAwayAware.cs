namespace Aci.Unity.UI.Navigation
{
    public interface INavigatedAwayAware : INavigationEvent
    {
        /// <summary>
        /// Called on page that is currently being navigated away from 
        /// </summary>
        /// <param name="parameters">Paramaters than can be passed to previous page</param>
        void OnNavigatedAway(INavigationParameters parameters);
    }
}
