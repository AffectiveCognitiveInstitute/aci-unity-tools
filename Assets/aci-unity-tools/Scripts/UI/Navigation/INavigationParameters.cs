namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    ///     Key-Value pairing which is used to pass parameters from screen to screen.
    /// </summary>
    public interface INavigationParameters
    {
        /// <summary>
        ///     Adds a key-value pair to the navigation parameters.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The name of the key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns the instance implementing <see cref="INavigationParameters"/>.</returns>
        INavigationParameters Add<T>(string key, T value);

        /// <summary>
        ///     Retrieves a value from the <see cref="INavigationParameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The name of the value.</param>
        /// <returns>Returns the value.</returns>
        T GetValue<T>(string key);

        /// <summary>
        ///     Tries to get the value by its key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The name of the key.</param>
        /// <param name="value">The value retrieved, if successful.</param>
        /// <returns>Returns <see langword="true"/> if successful, else <see langword="false"/>.</returns>
        bool TryGetValue<T>(string key, out T value);

        /// <summary>
        ///     Clears the navigation parameters.
        /// </summary>
        void Clear();
    }
}
