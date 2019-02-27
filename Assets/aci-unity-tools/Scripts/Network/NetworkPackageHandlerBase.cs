// <copyright file=NetworkPackageHandlerBase.cs/>
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
// <date>11/21/2018 14:23</date>

using System;
using UnityEngine;

namespace Aci.Unity.Network
{
    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T> : MonoBehaviour
                                                       , INetworkPackageHandler
    where T : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new [] { typeof(T) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Handle((T)package);
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }

    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T0"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T1"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T0, T1> : MonoBehaviour
                                                            , INetworkPackageHandler
    where T0 : INetworkPackage
    where T1 : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new[] { typeof(T0), typeof(T1) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T0 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T1 package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Type target = package.GetType();
            if (target == typeof(T0))
            {
                Handle((T0)package);
                return;
            }
            if (target == typeof(T1))
            {
                Handle((T1)package);
                return;
            }
            Aci.Unity.Logging.AciLog.LogWarning("NetworkPackageHandlerBase", "Received package of type" + package.GetType() + "not able to be handled by this handler.");
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }

    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T0"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T1"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T2"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T0,T1,T2> : MonoBehaviour
                                                              , INetworkPackageHandler
        where T0 : INetworkPackage
        where T1 : INetworkPackage
        where T2 : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new[] { typeof(T0), typeof(T1), typeof(T2) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T0 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T1 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T2 package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Type target = package.GetType();
            if (target == typeof(T0))
            {
                Handle((T0)package);
                return;
            }
            if (target == typeof(T1))
            {
                Handle((T1)package);
                return;
            }
            if (target == typeof(T2))
            {
                Handle((T2)package);
                return;
            }
            Aci.Unity.Logging.AciLog.LogWarning("NetworkPackageHandlerBase", "Received package of type" + package.GetType() + "not able to be handled by this handler.");
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }

    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T0"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T1"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T2"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T3"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T0,T1,T2,T3> : MonoBehaviour
                                                                 , INetworkPackageHandler
        where T0 : INetworkPackage
        where T1 : INetworkPackage
        where T2 : INetworkPackage
        where T3 : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T0 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T1 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T2 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T3 package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Type target = package.GetType();
            if (target == typeof(T0))
            {
                Handle((T0)package);
                return;
            }
            if (target == typeof(T1))
            {
                Handle((T1)package);
                return;
            }
            if (target == typeof(T2))
            {
                Handle((T2)package);
                return;
            }
            if (target == typeof(T3))
            {
                Handle((T3)package);
                return;
            }
            Aci.Unity.Logging.AciLog.LogWarning("NetworkPackageHandlerBase", "Received package of type" + package.GetType() + "not able to be handled by this handler.");
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }

    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T0"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T1"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T2"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T3"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T4"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T0, T1, T2, T3, T4> : MonoBehaviour
                                                                        , INetworkPackageHandler
        where T0 : INetworkPackage
        where T1 : INetworkPackage
        where T2 : INetworkPackage
        where T3 : INetworkPackage
        where T4 : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T0 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T1 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T2 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T3 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T4 package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Type target = package.GetType();
            if (target == typeof(T0))
            {
                Handle((T0)package);
                return;
            }
            if (target == typeof(T1))
            {
                Handle((T1)package);
                return;
            }
            if (target == typeof(T2))
            {
                Handle((T2)package);
                return;
            }
            if (target == typeof(T3))
            {
                Handle((T3)package);
                return;
            }
            if (target == typeof(T4))
            {
                Handle((T4)package);
                return;
            }
            Aci.Unity.Logging.AciLog.LogWarning("NetworkPackageHandlerBase", "Received package of type" + package.GetType() + "not able to be handled by this handler.");
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }

    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T0"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T1"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T2"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T3"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T4"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T5"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T0, T1, T2, T3, T4, T5> : MonoBehaviour
                                                                            , INetworkPackageHandler
        where T0 : INetworkPackage
        where T1 : INetworkPackage
        where T2 : INetworkPackage
        where T3 : INetworkPackage
        where T4 : INetworkPackage
        where T5 : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T0 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T1 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T2 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T3 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T4 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T5 package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Type target = package.GetType();
            if (target == typeof(T0))
            {
                Handle((T0)package);
                return;
            }
            if (target == typeof(T1))
            {
                Handle((T1)package);
                return;
            }
            if (target == typeof(T2))
            {
                Handle((T2)package);
                return;
            }
            if (target == typeof(T3))
            {
                Handle((T3)package);
                return;
            }
            if (target == typeof(T4))
            {
                Handle((T4)package);
                return;
            }
            if (target == typeof(T5))
            {
                Handle((T5)package);
                return;
            }
            Aci.Unity.Logging.AciLog.LogWarning("NetworkPackageHandlerBase", "Received package of type" + package.GetType() + "not able to be handled by this handler.");
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }


    /// <summary>
    ///     <see cref="MonoBehaviour"/> implementation of <see cref="INetworkPackageHandler"/>.
    ///     This is used to handle <see cref="INetworkPackage"/> instances received via a <see cref="NetworkSubscriber"/>.
    ///     A single <see cref="GameObject"/> can not have multiple <see cref="NetworkPackageHandlerBase{T}"/>-Components attached that handle the same <see cref="INetworkPackage"/> type.
    /// </summary>
    /// <typeparam name="T0"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T1"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T2"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T3"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T4"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T5"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    /// <typeparam name="T6"><see cref="INetworkPackage"/> implementation that should be handled by the Handler.</typeparam>
    public abstract class NetworkPackageHandlerBase<T0, T1, T2, T3, T4, T5, T6> : MonoBehaviour
                                                                            , INetworkPackageHandler
        where T0 : INetworkPackage
        where T1 : INetworkPackage
        where T2 : INetworkPackage
        where T3 : INetworkPackage
        where T4 : INetworkPackage
        where T5 : INetworkPackage
        where T6 : INetworkPackage
    {
        // parent registry instance, typically there should only be one
        private NetworkPackageHandlerRegistry _registry;

        /// <inheritdoc cref="INetworkPackageHandler"/>
        public Type[] packageTypes => new[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) , typeof(T6) };

        // Zenject injection point
        [Zenject.Inject]
        void Construct(NetworkPackageHandlerRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T0 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T1 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T2 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T3 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T4 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T5 package);

        /// <summary>
        ///     Handles a <see cref="INetworkPackage"/> instance.
        /// </summary>
        /// <param name="package">Target <see cref="INetworkPackage"/> instance.</param>
        public abstract void Handle(T6 package);

        /// <inheritdoc cref="INetworkPackageHandler"/>
        void INetworkPackageHandler.Handle(INetworkPackage package)
        {
            Type target = package.GetType();
            if (target == typeof(T0))
            {
                Handle((T0)package);
                return;
            }
            if (target == typeof(T1))
            {
                Handle((T1)package);
                return;
            }
            if (target == typeof(T2))
            {
                Handle((T2)package);
                return;
            }
            if (target == typeof(T3))
            {
                Handle((T3)package);
                return;
            }
            if (target == typeof(T4))
            {
                Handle((T4)package);
                return;
            }
            if (target == typeof(T5))
            {
                Handle((T5)package);
                return;
            }
            if (target == typeof(T6))
            {
                Handle((T6)package);
                return;
            }
            Aci.Unity.Logging.AciLog.LogWarning("NetworkPackageHandlerBase", "Received package of type" + package.GetType() + "not able to be handled by this handler.");
        }

        void OnEnable()
        {
            // adds the handler to the registry
            _registry.AddHandler(this);
        }

        void OnDisable()
        {
            // removes the handler from the registry
            _registry.RemoveHandler(this);
        }
    }
}