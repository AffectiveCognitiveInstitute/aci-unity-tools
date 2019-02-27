// <copyright file=UnityMainThreadDispatcher.cs/>
// <copyright>
//   Copyright 2015 Pim de Witte All Rights Reserved.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//   http: //www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <license>Apache License, Version 2.0</license>
// <license disclaimer>
//   The originally licensed file has been partially modified.
// </license disclaimer>
// <main contributors>
//   Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>11/21/2018 16:49</date>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aci.Unity.Util
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static UnityMainThreadDispatcher m_Instance = null;
        private static readonly Queue<Action> m_ExecutionQueue = new Queue<Action>();

        void Update()
        {
            lock (m_ExecutionQueue)
            {
                while (m_ExecutionQueue.Count > 0)
                {
                    m_ExecutionQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void Enqueue(IEnumerator action)
        {
            lock (m_ExecutionQueue)
            {
                m_ExecutionQueue.Enqueue(() => { StartCoroutine(action); });
            }
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }

        static bool Exists()
        {
            return m_Instance != null;
        }

        public static UnityMainThreadDispatcher Instance()
        {
            if (!Exists())
            {
                throw new Exception(
                    "UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
            }

            return m_Instance;
        }

        void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnDestroy()
        {
            m_Instance = null;
        }
    }
}