// <copyright file=AciLog.cs/>
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
// <date>11/23/2018 11:40</date>

using System;
using Aci.Unity.Network;
using Aci.Unity.Util;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Aci.Unity.Logging
{
    /// <summary>
    ///     Contains static logging methods that also send logs via a <see cref="INetworkPublisher" />. Capabilities are set at
    ///     stratup via Injection.
    ///     The <see cref="AciLog" /> class is not static to enable injection. The static methods access the singleton-instance
    ///     of the logging class.
    /// </summary>
    public class AciLog
    {
        //singleton
        private static AciLog  m_Instance;
        private        ILogger m_DefaultLogger;

        private INetworkPublisher m_NetPublisher;

        //Zenject injection point
        [Inject]
        public void Construct(INetworkPublisher publisher, ILogger logger)
        {
            m_DefaultLogger = logger;
            m_NetPublisher = publisher;
            m_Instance = this;
        }

        /// <summary>
        ///     Logs a message and a tag.
        /// </summary>
        /// <param name="tag">Tag to log.</param>
        /// <param name="message">Message to log.</param>
        public static void Log(string tag, object message)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.Log(tag, message); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Log.ToString(),
                tag = tag,
                message = message.ToString()
            });
        }

        /// <summary>
        ///     Logs a message, a tag and an object context.
        /// </summary>
        /// <param name="tag">Tag to log.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Object context.</param>
        public static void Log(string tag, object message, Object context)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.Log(tag, message, context); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Log.ToString(),
                tag = tag,
                message = message.ToString()
            });
        }

        /// <summary>
        ///     Logs a warning mesasge and a tag.
        /// </summary>
        /// <param name="tag">Tag to log.</param>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(string tag, object message)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogWarning(tag, message); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Warning.ToString(),
                tag = tag,
                message = message.ToString()
            });
        }

        /// <summary>
        ///     Logs a warning mesasge, a tag and an object context.
        /// </summary>
        /// <param name="tag">Tag to log.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Object context.</param>
        public static void LogWarning(string tag, object message, Object context)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogWarning(tag, message); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Warning.ToString(),
                tag = tag,
                message = message.ToString()
            });
        }

        /// <summary>
        ///     Logs an exception.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        public static void LogException(Exception exception)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogException(exception); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Exception.ToString(),
                tag = exception.Source,
                message = exception.Message
            });
        }

        /// <summary>
        ///     Logs an exception.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        /// <param name="context">Object context.</param>
        public static void LogException(Exception exception, Object context)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogException(exception, context); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Exception.ToString(),
                tag = exception.Source,
                message = exception.Message
            });
        }

        /// <summary>
        ///     Logs an Error message and a tag.
        /// </summary>
        /// <param name="tag">Tag to log.</param>
        /// <param name="message">Message to log.</param>
        public static void LogError(string tag, object message)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogError(tag, message); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Error.ToString(),
                tag = tag,
                message = message.ToString()
            });
        }

        /// <summary>
        ///     Logs an error message, a tag and an object context.
        /// </summary>
        /// <param name="tag">Tag to log.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Object context.</param>
        public static void LogError(string tag, object message, Object context)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogError(tag, message, context); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = LogType.Error.ToString(),
                tag = tag,
                message = message.ToString()
            });
        }

        /// <summary>
        ///     Logs a formatted message with a tag.
        /// </summary>
        /// <param name="logType">Target Unity <see cref="LogType" />.</param>
        /// <param name="tag">Tag to log.</param>
        /// <param name="format">Formatted message to log.</param>
        /// <param name="args">Formatting arguments.</param>
        public static void LogFormat(LogType logType, string tag, string format, params object[] args)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => { m_Instance?.m_DefaultLogger?.LogFormat(logType, format, args); });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = logType.ToString(),
                tag = tag,
                message = string.Format(format, args)
            });
        }

        /// <summary>
        ///     Logs a formatted message with a tag and object context.
        /// </summary>
        /// <param name="logType">Target Unity <see cref="LogType" />.</param>
        /// <param name="context">Object context.</param>
        /// <param name="tag">Tag to log.</param>
        /// <param name="format">Formatted message to log.</param>
        /// <param name="args">Formatting arguments.</param>
        public static void LogFormat(LogType logType, Object context, string tag, string format, params object[] args)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                             {
                                                 m_Instance?.m_DefaultLogger?.LogFormat(logType, context, format, args);
                                             });
            m_Instance?.m_NetPublisher?.Send(new LoggingPackage
            {
                timeStamp = DateTime.Now.ToShortTimeString(),
                type = logType.ToString(),
                tag = tag,
                message = string.Format(format, args)
            });
        }
    }
}