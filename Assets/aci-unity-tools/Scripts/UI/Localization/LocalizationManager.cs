// <copyright file=LocalizationManager.cs/>
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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aci.Unity.Events;
using Aci.Unity.Logging;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.Localization
{
    [ExecuteInEditMode]
    public class LocalizationManager : MonoBehaviour, ILocalizationManager
    {
        private bool initialized = false;

        //buffer for fast access
        private LocalizationData currentLocalizationData = null;

        //serialized value for restoring values
        [SerializeField]
        private string _currentLocalization = null;

        [Inject]
        private IAciEventManager eventBroker;

        public List<LocalizationData> baseLocalization = new List<LocalizationData>();

        private List<LocalizationData> loadedData = new List<LocalizationData>();

        /// <summary>
        ///     A List of several replacement patterns for localized strings. The asterisk will be replaced by the identifier.
        ///     Additional patterns can be provided via the inspector editor.
        /// </summary>
        public string[] supportedReplacementPatterns = new[]
        {
            "{{(.+?)}}",
            "##(.+?)##",
            "<<(.+?)>>"
        };

        [SerializeField]
        private string _replacementPattern;
        
        /// <inheritdoc />
        public string replacementPattern
        {
            get { return _replacementPattern; }
            set
            {
                if (supportedReplacementPatterns.Contains(value))
                    _replacementPattern = value;
            }
        }

        /// <inheritdoc />
        public string currentLocalization
        {
            get { return _currentLocalization; }
            set
            {
                if (loadedData.Count == 0)
                    if (value == null)
                    {
                        _currentLocalization = null;
                        currentLocalizationData = null;
                        return;
                    }

                foreach (LocalizationData data in loadedData)
                    if (data.languageIETF == value)
                    {
                        _currentLocalization = value;
                        currentLocalizationData = data;
                        if (eventBroker != null)
                            eventBroker.Invoke(new LocalizationChangedArgs()
                            {
                                ietf = value,
                                localeDecorator = currentLocalizationData.languageDescriptor
                            });
                        return;
                    }

                AciLog.LogFormat(LogType.Error, "LocalizationManager", "Target language \"{0}\" is not loaded.", value);
            }
        }

        /// <inheritdoc />
        public string currentLocalizationDecorator => currentLocalizationData?.languageDescriptor;

        // Initialization function to restore values from serialized object
        private void Intialize()
        {
            foreach (LocalizationData data in baseLocalization)
            {
                AddLocalizationData(data);
            }
            foreach (LocalizationData data in loadedData)
                if (data.languageIETF == _currentLocalization)
                {
                    currentLocalizationData = data;
                    if (eventBroker != null)
                        eventBroker.Invoke(new LocalizationChangedArgs()
                        {
                            ietf = _currentLocalization,
                            localeDecorator = currentLocalizationData.languageDescriptor
                        });
                }

            if (currentLocalizationData == null)
            {
                AciLog.LogFormat(LogType.Error, "LocalizationManager", "Target language \"{0}\" could not be loaded from persivously serialzed values. Please check if the correct files are set on the LocalizationManager instance.", _currentLocalization);
                _currentLocalization = null;
            }

            initialized = true;
        }

        /// <inheritdoc />
        public bool IsLocalized(string str)
        {
            MatchCollection col = Regex.Matches(str, replacementPattern);
            return col.Count > 0;
        }

        /// <inheritdoc />
        public string GetLocalized(string targetString)
        {
            if (currentLocalizationData == null)
                return targetString;
            MatchCollection col = Regex.Matches(targetString, replacementPattern);
            foreach (Match match in col)
            {
                string val = null;
                if (!currentLocalizationData.stringData.TryGetValue(match.Value.Substring(2, match.Length - 4), out val)
                )
                    continue;
                targetString = targetString.Replace(match.Value, val);
            }
            return targetString;
        }

        /// <inheritdoc />
        public List<string> GetCapabilities()
        {
            List<string> ietfList = new List<string>();
            foreach (LocalizationData data in loadedData)
                ietfList.Add(data.languageIETF);
            return ietfList;
        }

        /// <inheritdoc />
        public bool AddLocalizationData(LocalizationData data)
        {
            LocalizationData mergeTarget = null;
            if (GetCapabilities().Contains(data.languageIETF))
            {
                foreach (LocalizationData dataTarget in loadedData)
                {
                    if (data.languageIETF != dataTarget.languageIETF)
                        continue;
                    mergeTarget = dataTarget;
                    break;
                }
            }
            else
            {
                mergeTarget = ScriptableObject.CreateInstance<LocalizationData>();
                mergeTarget.languageDescriptor = data.languageDescriptor;
                mergeTarget.languageIETF = data.languageIETF;
                loadedData.Add(mergeTarget);
            }

            if (mergeTarget == null)
                return false;

            return mergeTarget.AddData(data);
        }

        /// <inheritdoc />
        public bool RemoveLocalizationData(LocalizationData data)
        {
            if (!GetCapabilities().Contains(data.languageIETF))
                return false;

            LocalizationData subtractionTarget = null;
            foreach (LocalizationData dataTarget in loadedData)
            {
                if (data.languageIETF != dataTarget.languageIETF)
                    continue;
                subtractionTarget = dataTarget;
                break;
            }

            if (subtractionTarget == null)
                return false;

            return subtractionTarget.SubtractData(data);
        }

        /// <inheritdoc />
        public async void LoadLocalizationFile(string url)
        {
            //check if file is actual json file
            if (!url.EndsWith(".json"))
                return;

            WWW target = new WWW(url);
            while (!target.isDone)
            {
                await Task.Delay(1000);
            }

            string[] parts = url.Split(new [] {"."}, StringSplitOptions.None);
            LocalizationData data = ScriptableObject.CreateInstance<LocalizationData>();
            CultureInfo info = CultureInfo.GetCultureInfoByIetfLanguageTag(parts[parts.Length - 2]);
            data.languageIETF = info.IetfLanguageTag;
            data.languageDescriptor = info.ThreeLetterISOLanguageName;
            data.FromJSON(target.text);

            AddLocalizationData(data);
        }

        /// <inheritdoc />
        public void ClearData()
        {
            loadedData.Clear();
            currentLocalizationData = null;
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!Application.isPlaying && !initialized)
                Intialize();
        }
#endif

        private void Start()
        {
            if (!initialized)
                Intialize();
        }
    }
}