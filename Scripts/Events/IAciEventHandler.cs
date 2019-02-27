using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.Unity.Events
{
    /// <summary>
    ///     Interface for classes that should handle AciEvents. This should be implemented by a specific event handler
    ///     interface.
    /// </summary>
    public interface IAciEventHandler<T> where T : struct
    {
        /// <summary>
        ///     Registers all needed events with a broker
        /// </summary>
        void RegisterForEvents();

        /// <summary>
        ///     Unregisters all registered events with a broker
        /// </summary>
        void UnregisterFromEvents();

        /// <summary>
        ///     Event Callback for registered events.
        /// </summary>
        /// <param name="arg">Event payload.</param>
        void OnEvent(T arg);
    }
}
