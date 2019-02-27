// <copyright file=AciEventHandler.cs/>
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
// <date>07/16/2018 17:48</date>

using System;

namespace Aci.Unity.Events.deprecated
{
    /// <summary>
    ///     Interface for classes that should handle AciEvents. This should be implemented by a specific event handler
    ///     interface.
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface IAciEventHandler
    {
        /// <summary>
        ///     Registers all needed events with a broker
        /// </summary>
        void RegisterForEvents();

        /// <summary>
        ///     Unregisters all registered events with a broker
        /// </summary>
        void UnregisterFromEvents();
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="UserLoginEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface ILoginHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event callback for <see cref="UserLoginEvent" />
        /// </summary>
        /// <param name="result">true if login succeeded, false otherwise</param>
        /// <param name="message">null if login succeeded, otherwise contains error message</param>
        void OnUserLogin(bool result, string message);
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="UserLogoutEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface ILogoutHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event callback for <see cref="UserLogoutEvent" />
        /// </summary>
        /// <param name="result">true if login succeeded, false otherwise</param>
        /// <param name="message">null if login succeeded, otherwise contains error message</param>
        void OnUserLogout();
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="UserRegisterEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface UserRegisterHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event callback for <see cref="UserRegisterEvent" />
        /// </summary>
        /// <param name="result">true if registration succeeded, false otherwise</param>
        /// <param name="message">null if registration succeeded, otherwise contains error message</param>
        void OnUserRegister(bool result, string message);
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="UserUpdateEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface IUpdateHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event callback for <see cref="UserUpdateEvent" />
        /// </summary>
        /// <param name="result">true if update succeeded, false otherwise</param>
        /// <param name="message">null if update succeeded, otherwise contains error message</param>
        void OnUserUpdate(bool result, string message);
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="DbUpdateEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface IDbUpdateHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event callback for <see cref="DbUpdateEvent" />
        /// </summary>
        /// <param name="result">true if update succeeded, false otherwise</param>
        void OnDbUpdate(bool result);
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="DbRemoveEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface IDbRemoveHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event callback for <see cref="DbRemoveEvent" />
        /// </summary>
        /// <param name="result">true if remove succeeded, false otherwise</param>
        /// <param name="message">null if remove succeeded, otherwise contains error message</param>
        void OnDbRemove(bool result, string message);
    }

    /// <summary>
    ///     Interface for classes that should handle <see cref="LocalizationChangedEvent" />
    /// </summary>
    [Obsolete("Deprecated, please use new IAciEventHandler<T>.")]
    public interface ILocalizationChangedHandler : IAciEventHandler
    {
        /// <summary>
        ///     Event Callback for <see cref="LocalizationChangedEvent" />
        /// </summary>
        /// <param name="localeID">Localization ietf identifier.</param>
        /// <param name="localeDecorator">Localization ISO 639-2 identifier.</param>
        void OnLocalizationChanged(string localeID, string localeDecorator);
    }
}