using System.Threading.Tasks;

namespace Aci.Unity.UI
{
    /// <summary>
    ///     Represents an UI Transition.
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        ///     Starts the enter transition.
        /// </summary>
        /// <returns>
        ///     Returns an awaitable <see cref="Task"/> which completes when the enter
        ///     transition has completed.
        /// </returns>
        Task EnterAsync();

        /// <summary>
        ///     Starts the exit transition.
        /// </summary>
        /// <returns>
        ///     Returns an awaitable <see cref="Task"/> which completes when the exit
        ///     transition has completed.
        /// </returns>
        Task ExitAsync();
    }
}
