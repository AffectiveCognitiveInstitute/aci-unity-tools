using System;
using System.Threading.Tasks;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    ///     Controls visibility of screen.
    /// </summary>
    public interface IScreenController : IEquatable<IScreenController>, INavigationAware
    {
        /// <summary>
        ///     The id of the screen.
        /// </summary>
        string id { get; }

        /// <summary>
        ///     Prepares the screen for navigation.
        /// </summary>
        void Prepare();

        /// <summary>
        ///     Updates the screen based on the navigation mode.
        /// </summary>
        /// <param name="navigationMode">The <see cref="NavigationMode"/>.</param>
        /// <param name="animated">Should the transition be animated?</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task UpdateDisplayAsync(NavigationMode navigationMode, bool animated = true);
    }
}
