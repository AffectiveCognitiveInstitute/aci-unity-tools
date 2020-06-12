namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Describes whether a navigation can be executed.
    /// </summary>
    public interface ICanNavigate
    {
        /// <summary>
        ///     Determines whether the instance accepts being navigated away from.
        /// </summary>
        /// <param name="navigationParameters">The navigation parameters.</param>
        /// <returns><c>True</c> if navigation can continue, <c>False</c> if it cannot.</returns>
        bool CanNavigate(INavigationParameters navigationParameters);
    }
}