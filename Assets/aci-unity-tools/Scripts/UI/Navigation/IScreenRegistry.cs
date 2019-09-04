namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    ///     Registry for <see cref="IScreenController"/>.
    /// </summary>
    public interface IScreenRegistry
    {
        /// <summary>
        ///     Registers an <see cref="IScreenController"/>.
        /// </summary>
        /// <param name="screen">The <see cref="IScreenController"/> to register.</param>
        void Register(IScreenController screen);

        /// <summary>
        ///     Unregisters an <see cref="IScreenController"/>.
        /// </summary>
        /// <param name="screen">The <see cref="IScreenController"/> to unregister.</param>
        void Unregister(IScreenController screen);

        /// <summary>
        ///     Tries to get an <see cref="IScreenController"/> by its id.
        /// </summary>
        /// <param name="screenId">The id of the <see cref="IScreenController"/>.</param>
        /// <param name="screen">The retrieved <see cref="IScreenController"/>.</param>
        /// <returns>Returns <see langword="true"/> if successful, else <see langword="false"/>.</returns>
        bool TryGetScreen(string screenId, out IScreenController screen);
    }
}
