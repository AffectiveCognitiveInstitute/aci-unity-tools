using System.Threading.Tasks;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Service which exposes methods for navigating within application
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Can a Navigation Pop command be executed?
        /// </summary>
        /// <returns>Returns true or false</returns>
        bool CanPop();

        /// <summary>
        /// Switch to a screen
        /// </summary>
        /// <param name="screen">The identifier of the screen to be pushes</param>
        /// <param name="animationOptions">Animation options for the possible screen being hidden and for the screen being pushed</param>
        /// <param name="addToHistory">Should the previous screen be added onto the navigation stack?</param>
        /// <returns>Returns an awaitable Task. The Task completes when the animation has completed</returns>
        Task PushAsync(string screen, AnimationOptions animationOptions, bool addToHistory = true);

        /// <summary>
        /// Switch to a screen
        /// </summary>
        /// <param name="screen">The identifier of the screen to be pushed</param>
        /// <param name="parameters">Any parameters that should be passed to next screen.</param>
        /// <param name="animationOptions">Animation options for the possible screen being hidden and for the screen being pushed</param>
        /// <param name="addToHistory">Should the previous screen be added onto the navigation stack?</param>
        /// <returns>Returns an awaitable Task. The Task completes when the animation has completed</returns>
        Task PushAsync(string screen, INavigationParameters parameters, AnimationOptions animationOptions, bool addToHistory = true);

        /// <summary>
        /// Exits the current screen. Goes back to the previous screen
        /// </summary>
        /// <param name="animationOptions">Animation options for the screen being popped and for the screen that reappears</param>
        /// <returns>Returns an awaitable Task. The Task completes when the animation has completed</returns>
        Task PopAsync(AnimationOptions animationOptions);

        /// <summary>
        /// Exits the current screen. Goes back to the previous screen
        /// </summary>
        /// <param name="parameters">Any values that should be passed back to the previous screen.</param>
        /// <param name="animationOptions">Animation options for the screen being popped and for the screen that reappears</param>
        /// <returns>Returns an awaitable Task. The Task completes when the animation has completed</returns>
        Task PopAsync(INavigationParameters parameters, AnimationOptions animationOptions);

        /// <summary>
        /// Pops to the first screen
        /// </summary>
        /// <param name="animationOptions">Animation options for the screen being popped and for the screen that reappears</param>
        /// <returns>Returns an awaitable Task</returns>
        Task PopToRootAsync(AnimationOptions animationOptions);


        /// <summary>
        /// Pops to the first screen
        /// </summary>
        /// <param name="parameters">Any values that should be passed back to the root screen.</param>
        /// <param name="animationOptions">Animation options for the screen being popped and for the screen that reappears</param>
        /// <returns>Returns an awaitable Task</returns>
        Task PopToRootAsync(INavigationParameters parameters, AnimationOptions animationOptions);


        /// <summary>
        ///     Pushes a screen and starts a new navigation stack. This will destroy any screens present on the current stack.
        /// </summary>
        /// <param name="screen">The identifier of the screen to be pushed.</param>
        /// <param name="parameters">Any values that should be passed to the new new screens.</param>
        /// <param name="animationOptions">Animation options for the screen being popped and for the screen that reappears.</param>
        /// <returns></returns>
        Task PushWithNewStackAsync(string screen, INavigationParameters parameters, AnimationOptions animationOptions);
    }
}