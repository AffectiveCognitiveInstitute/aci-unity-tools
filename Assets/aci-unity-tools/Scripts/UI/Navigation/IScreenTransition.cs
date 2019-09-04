using System.Threading.Tasks;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    ///     Interface for screen transitions.
    /// </summary>
    public interface IScreenTransition
    {
        /// <summary>
        ///     Makes a transition based on the <see cref="NavigationMode"/>.
        /// </summary>
        /// <param name="navigationMode">The type of <see cref="NavigationMode"/>.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        Task MakeTransitionAsync(NavigationMode navigationMode);

        /// <summary>
        ///     Transition instantly.
        /// </summary>
        void DisplayImmediately();
    }
}