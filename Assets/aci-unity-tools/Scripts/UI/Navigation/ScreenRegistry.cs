using System;
using System.Collections.Generic;

namespace Aci.Unity.UI.Navigation
{
    public class ScreenRegistry : IScreenRegistry
    {
        private readonly Dictionary<string, IScreenController> m_Screens = new Dictionary<string, IScreenController>(10);

        /// <inheritdoc />
        public void Register(IScreenController screen)
        {
            if(screen == null)
                throw new ArgumentNullException(nameof(screen));

            if (string.IsNullOrEmpty(screen.id))
                throw new ArgumentNullException(nameof(screen.id), "Screen ID cannot be null or empty!");

            m_Screens.Add(screen.id, screen);            
        }

        /// <inheritdoc />
        public bool TryGetScreen(string screenId, out IScreenController screen)
        {
            if (string.IsNullOrEmpty(screenId))
                throw new ArgumentNullException(nameof(screenId));

            return m_Screens.TryGetValue(screenId, out screen);
        }

        /// <inheritdoc />
        public void Unregister(IScreenController screen)
        {
            if (screen == null)
                throw new ArgumentNullException(nameof(screen));

            if (string.IsNullOrEmpty(screen.id))
                throw new ArgumentNullException(nameof(screen.id), "Screen ID cannot be null or empty!");

            m_Screens.Remove(screen.id);
        }
    }
}
