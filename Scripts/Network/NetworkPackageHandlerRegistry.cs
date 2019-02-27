// <copyright file=NetworkPackageHandlerRegistry.cs/>
// <copyright>
//   Copyright (c) 2018, Affective & Cognitive Institute
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software andassociated documentation files
//   (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
//   merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//   OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
//   IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// </copyright>
// <license>MIT License</license>
// <main contributors>
//   Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>11/21/2018 14:10</date>

using System;
using System.Collections.Generic;

namespace Aci.Unity.Network
{
    /// <summary>
    ///     A registry of <see cref="INetworkPackageHandler"/> instances. Used to assign received <see cref="INetworkPackage"/> instances to their correct handlers.
    ///     Multiple Handlers can be registered for each <see cref="INetworkPackage"/> implementation.
    ///     Handlers are identified via the <see cref="INetworkPackageHandler.packageType"/> property which is compared to the type of incoming <see cref="INetworkPackage"/> instances.
    /// </summary>
    public class NetworkPackageHandlerRegistry
    {
        // package type - handler binding
        private readonly Dictionary<Type, List<INetworkPackageHandler>> _handlers = new Dictionary<Type, List<INetworkPackageHandler>>();

        // Zenject injection point
        [Zenject.Inject]
        void Construct(List<INetworkPackageHandler> handlers)
        {
            foreach(INetworkPackageHandler handler in handlers)
                AddHandler(handler);
        }
        
        /// <summary>
        ///     Adds a handler to the registry. Adding a handler multiple times is prevented.
        /// </summary>
        /// <param name="handler">Handler to add.</param>
        public void AddHandler(INetworkPackageHandler handler)
        {
            foreach (Type packageType in handler.packageTypes)
            {
                List<INetworkPackageHandler> handlers;
                if (!_handlers.TryGetValue(packageType, out handlers))
                {
                    _handlers[packageType] = new List<INetworkPackageHandler>();
                    handlers = _handlers[packageType];
                }
                if (handlers.Contains(handler))
                    continue;
                handlers.Add(handler);
            }
        }

        /// <summary>
        ///     Removes a handler from the registry, if possible.
        /// </summary>
        /// <param name="handler">Handler to remove.</param>
        public void RemoveHandler(INetworkPackageHandler handler)
        {
            foreach (Type packageType in handler.packageTypes)
            {
                List<INetworkPackageHandler> handlers;
                if (!_handlers.TryGetValue(packageType, out handlers) || !handlers.Contains(handler))
                    continue;
                handlers.Remove(handler);
            }
        }

        /// <summary>
        ///     Handles a package.
        /// </summary>
        /// <typeparam name="T">Target package type.</typeparam>
        /// <param name="package">Target package.</param>
        public void Handle<T>(T package) where T : INetworkPackage
        {
            List<INetworkPackageHandler> handlers;
            if (!_handlers.TryGetValue(package.GetType(), out handlers))
                return;
            foreach(INetworkPackageHandler handler in handlers)
                handler.Handle(package);
        }
    }
}