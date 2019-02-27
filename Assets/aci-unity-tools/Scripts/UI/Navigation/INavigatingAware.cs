namespace Aci.Unity.UI.Navigation
{
    public interface INavigatingAware : INavigationEvent
    {
        /// <summary>
        /// Called while navigating to the page. Called before any animation takes place.
        /// </summary>
        /// <param name="parameters"></param>
        void OnNavigatingTo(INavigationParameters parameters);
    }
}
