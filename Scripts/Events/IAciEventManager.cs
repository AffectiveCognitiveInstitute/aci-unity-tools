using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aci.Unity.Events
{
    /// <summary>
    ///     Interface for the global event manager.
    /// </summary>
    public interface IAciEventManager
    {
        /// <summary>
        ///     Checks if handler can actually handle this event type.
        /// </summary>
        /// <typeparam name="T">Target event's EventArgumentStruct.</typeparam>
        /// <returns>Ture if event manager can handle event, false otherwise.</returns>
        bool canHandleEvent<T>() where T : struct;

        /// <summary>
        ///     Invokes an event of the struct type.
        /// </summary>
        /// <typeparam name="T">Target event's EventArgumentStruct.</typeparam>
        /// <param name="args">EventArgumentStruct instance.</param>
        void Invoke<T>(T args) where T : struct;

        /// <summary>
        ///     Adds an <see cref="IAciEventHandler{T}"/> instance for subscription.
        /// </summary>
        /// <typeparam name="T">Target event's EventArgumentStruct.</typeparam>
        /// <param name="handler">Target <see cref="IAciEventHandler{T}"/> instance.</param>
        void AddHandler<T>(IAciEventHandler<T> handler) where T : struct;

        /// <summary>
        ///     Removes an <see cref="IAciEventHandler{T}"/> instance currently subscribed to an event.
        /// </summary>
        /// <typeparam name="T">Target event's EventArgumentStruct.</typeparam>
        /// <param name="handler">Target <see cref="IAciEventHandler{T}"/> instance.</param>
        void RemoveHandler<T>(IAciEventHandler<T> handler) where T : struct;
    }
}
