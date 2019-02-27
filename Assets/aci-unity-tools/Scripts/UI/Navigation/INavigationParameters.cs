using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Used for passing data between pages
    /// </summary>
    public interface INavigationParameters : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Adds a parameter 
        /// </summary>
        /// <param name="key">The key of the parameter</param>
        /// <param name="value">The parameter value</param>
        void Add(string key, object value);

        /// <summary>
        /// Checks if a key is contained in this collection
        /// </summary>
        /// <param name="key">The key to be checked</param>
        /// <returns>Returns true if this contains the given key</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Returns the number of parameters
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value by key
        /// </summary>
        /// <typeparam name="T">The type expected</typeparam>
        /// <param name="key">The key of the parameter</param>
        /// <returns></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// Tries to get a value by key
        /// </summary>
        /// <typeparam name="T">The type of key expected</typeparam>
        /// <param name="key">The key of the parameter</param>
        /// <param name="value">The value if the key is found</param>
        /// <returns>Returns true if the value can be found</returns>
        bool TryGetValue<T>(string key, out T value);     
    }
}

