using System.Collections;
using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    /// <summary>
    /// Implementation of <see cref="INavigationParameters"/>
    /// </summary>
    public class NavigationParameters : INavigationParameters, IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> m_InternalDict = new Dictionary<string, object>();

        /// <inheritdoc />
        public int Count
        {
            get { return m_InternalDict.Count; }
        } 

        /// <inheritdoc />
        public void Add(string key, object value)
        {
            m_InternalDict.Add(key, value);
        }

        /// <inheritdoc />
        public bool ContainsKey(string key)
        {
            return m_InternalDict.ContainsKey(key);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return m_InternalDict.GetEnumerator();
        }

        /// <inheritdoc />
        public T GetValue<T>(string key)
        {
            return (T) m_InternalDict[key];
        }

        /// <inheritdoc />
        public bool TryGetValue<T>(string key, out T value)
        {
            value = default(T);
            object internalVal = null;
            if(m_InternalDict.TryGetValue(key, out internalVal))
            {
                value = (T)internalVal;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
