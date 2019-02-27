// <copyright file=SerializableDictionary.cs/>
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
// <date>08/01/2018 06:16</date>

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aci.Unity.Util
{
    /// <summary>
    /// Dictionary wrapper to make dictionaries serializable by Unity.
    /// Mainly used for inspector bindings.
    /// </summary>
    /// <typeparam name="TK">Target key type.</typeparam>
    /// <typeparam name="TV">Target value type.</typeparam>
    [Serializable]
    public class SerializableDictionary<TK, TV> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<TK> keys = new List<TK>();

        [SerializeField] private List<TV> values = new List<TV>();

        /// <summary>
        /// Wrapped dictionary instance.
        /// </summary>
        public Dictionary<TK, TV> Dictionary { get; } = new Dictionary<TK, TV>();

        /// <inheritdoc/>
        public void OnAfterDeserialize()
        {
            Dictionary.Clear();

            for (int i = 0; i < keys.Count; ++i) Dictionary.Add(keys[i], values[i]);
        }

        /// <inheritdoc/>
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            keys = Dictionary.Keys.ToList();
            values = Dictionary.Values.ToList();
        }
    }
}