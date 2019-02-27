using System;
using System.Collections.Generic;
using System.Diagnostics;
using Zenject;

namespace Aci.Unity.Events
{
    /// <summary>
    ///     Implementation of IACIEventManager.
    /// </summary>
    public class AciEventManager : IAciEventManager
    {
        public delegate void EventDelegate<T>(T item);
        private class EventRegistry : Dictionary<Type, Delegate> {};
        
        private EventRegistry registry = new EventRegistry();

        /// <inheritdoc />
        public bool canHandleEvent<T>() where T : struct
        {
            return registry.ContainsKey(typeof(T));
        }

        /// <inheritdoc />
        public void Invoke<T>(T args) where T : struct
        {
            // get event id
            Delegate eventInstance = null;
            if (!registry.TryGetValue(args.GetType(), out eventInstance))
            {
                // return if we don't have subscribers
                return;
            }
            (eventInstance as EventDelegate<T>)?.Invoke(args);
        }

        /// <inheritdoc />
        public void AddHandler<T>(IAciEventHandler<T> handler) where T : struct
        {
            Type eventArgType = typeof(T);
            Delegate eventInstance = null;
            if (!registry.TryGetValue(eventArgType, out eventInstance))
            {
                registry[eventArgType] = new EventDelegate<T>(handler.OnEvent);
                return;
            }
            EventDelegate<T> del = eventInstance as EventDelegate<T>;
            del += handler.OnEvent;
            registry[eventArgType] = del;
        }

        /// <inheritdoc />
        public void RemoveHandler<T>(IAciEventHandler<T> handler) where T : struct
        {
            Type eventArgType = typeof(T);
            Delegate eventInstance = null;
            if (!registry.TryGetValue(eventArgType, out eventInstance))
            {
                // we don't have a valid event instance
                return;
            }
            EventDelegate<T> del = eventInstance as EventDelegate<T>;
            del -= handler.OnEvent;
            registry[eventArgType] = del;
            // if we don't have any subscribers remove event handler
            if (del != null)
                return;
            registry.Remove(eventArgType);

        }
    }
}
