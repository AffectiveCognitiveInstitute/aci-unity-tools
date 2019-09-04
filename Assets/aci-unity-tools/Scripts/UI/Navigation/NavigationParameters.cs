using System;
using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    public class NavigationParameters : INavigationParameters
    {
        private readonly Dictionary<string, object> m_Parameters = new Dictionary<string, object>();

        /// <inheritdoc />
        public INavigationParameters Add<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            m_Parameters.Add(key, value);

            return this;
        }

        /// <inheritdoc />
        public void Clear()
        {
            m_Parameters.Clear();
        }

        /// <inheritdoc />
        public T GetValue<T>(string key)
        {
            return (T) m_Parameters[key];
        }

        /// <inheritdoc />
        public bool TryGetValue<T>(string key, out T value)
        {
            object v = null;

           if(m_Parameters.TryGetValue(key, out v))
            {
                value = (T)v;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}
