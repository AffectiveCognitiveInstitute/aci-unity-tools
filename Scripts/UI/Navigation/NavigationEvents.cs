namespace Aci.Unity.UI.Navigation
{
    public interface INavigationEventHandler { }

    /// <summary>
    ///     Called on GameObject that is being navigated to. Called before animation begins. 
    /// </summary>
    public interface INavigatingAware : INavigationEventHandler
    {
        void OnNavigatingTo(INavigationParameters navigationParameters);
    }

    /// <summary>
    ///     Called on GameObject that has been navigated to. Called after animation has completd.
    /// </summary>
    public interface INavigatedAware : INavigationEventHandler
    {
        void OnNavigatedTo(INavigationParameters navigationParameters);
    }

    /// <summary>
    ///     Called on GameObject that is being returned to. Called before animation begins.
    /// </summary>
    public interface INavigatingBackAware : INavigationEventHandler
    {
        void OnNavigatingBack(INavigationParameters navigationParameters);
    }

    /// <summary>
    ///     Called on GameObject that is returned to. Called after animation completes.
    /// </summary>
    public interface INavigatedBackAware : INavigationEventHandler
    {
        void OnNavigatedBack(INavigationParameters navigationParameters);
    }

    /// <summary>
    ///     Called on GameObject that is being navigated away from. Called before animation begins.
    /// </summary>
    public interface INavigatingAwayAware : INavigationEventHandler
    {
        void OnNavigatingAway(INavigationParameters navigationParameters);
    }

    /// <summary>
    ///     Called on GameObject which is about to be destroyed. Called before animation begins.
    /// </summary>
    public interface IDestructibleAware : INavigationEventHandler
    {
        void OnScreenDestroyed(INavigationParameters navigationParameters);
    }

    /// <summary>
    ///     Interface which combines all navigation events
    /// </summary>
    public interface INavigationAware : INavigatedAware, INavigatingAware, INavigatingBackAware, INavigatingAwayAware, IDestructibleAware, INavigatedBackAware { }
}
