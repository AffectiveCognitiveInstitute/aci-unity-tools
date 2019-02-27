// <copyright file=LocalizationData.cs/>
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
using System.IO;
using Aci.Unity.Util;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Aci.Unity.UI.Localization
{
    [Serializable]
    public class LocalizationDictionary : SerializableDictionary<string, string>
    {
    }

    /// <summary>
    ///     DataModel for loaded localization strings. Can be added or subtracted from each other.
    /// </summary>
    [Serializable]
    public class LocalizationData : ScriptableObject
    {
        [Header("Strings"), SerializeField]
        private LocalizationDictionary internalStringData = new LocalizationDictionary();

        /// <summary>
        ///     Short code for language name. Used for display in UI. Should be compliant to ISO 639-2.
        /// </summary>
        [Header("UI representation")] public string languageDescriptor;

        /// <summary>
        ///     IETF tag used for language identification. Should be a tag comprised of ISO 639-1 language tag + a hyphen + ISO
        ///     3166-1 alpha-2 regional tag
        /// </summary>
        [Header("IETF tag")] public string languageIETF;

        /// <summary>
        ///     SerializationData as string key-value pairs of string identifier and localized string.
        /// </summary>
        public Dictionary<string, string> stringData => internalStringData.Dictionary;

        /// <summary>
        ///     SerializationData reference count. Indicates how many sources have added a string to this data instance.
        /// </summary>
        private Dictionary<string, uint> dataRefCount = new Dictionary<string, uint>();

        /// <summary>
        ///     Adds data from another data source to this instance.
        /// </summary>
        /// <param name="data">Target data to incorporate into </param>
        /// <returns>True if succeeds, False otherwise.</returns>
        public bool AddData(LocalizationData data)
        {
            //check if language is the same
            if (languageIETF != data.languageIETF)
                return false;
            //for each string
            foreach (KeyValuePair<string, string> kvp in data.stringData)
            {
                //if string does not exist
                if (!stringData.ContainsKey(kvp.Key))
                {
                    //add string
                    stringData[kvp.Key] = kvp.Value;
                    //add ref count
                    dataRefCount[kvp.Key] = 0;
                }
                //increment ref count
                ++dataRefCount[kvp.Key];
            }
            return true;
        }

        /// <summary>
        ///     Removes data using another source as reference.
        /// </summary>
        /// <param name="data">Target reference data.</param>
        /// <returns>True if succeeds, False otherwise.</returns>
        public bool SubtractData(LocalizationData data)
        {
            //check if language is the same
            if (languageIETF != data.languageIETF)
                return false;
            //check if data contains all the keys
            foreach (KeyValuePair<string, string> kvp in data.stringData)
            {
                if (!stringData.ContainsKey(kvp.Key))
                    return false;
            }
            //for each key to remove
            foreach (KeyValuePair<string, string> kvp in data.stringData)
            {
                if (--dataRefCount[kvp.Key] == 0)
                    stringData.Remove(kvp.Key);
            }
            return true;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(stringData, Formatting.Indented);
        }

        public void FromJSON(string data)
        {
            stringData.Clear();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            foreach(KeyValuePair<string, string> kvp in dict)
                stringData.Add(kvp.Key, kvp.Value);
        }
    }
}