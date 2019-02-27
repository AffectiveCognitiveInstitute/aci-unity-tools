// <copyright file=LocalizedString.cs/>
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

using Aci.Unity.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Aci.Unity.UI.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedString : MonoBehaviour
                                 , IAciEventHandler<LocalizationChangedArgs>
    {
        private IAciEventManager __broker;
        private string         bufferedStringID;

        [Inject]
        private ILocalizationManager localizationManager;

        private TextMeshProUGUI textMesh;

        [Inject]
        private IAciEventManager broker
        {
            get { return __broker; }
            set
            {
                if (value == null)
                    return;
                UnregisterFromEvents();
                __broker = value;
                RegisterForEvents();
            }
        }

        // Cleanup in case of object destruction by unity
        void OnDestroy()
        {
            UnregisterFromEvents();
        }

        /// <inheritdoc />
        public void RegisterForEvents()
        {
            __broker?.AddHandler<LocalizationChangedArgs>(this);
        }

        /// <inheritdoc />
        public void UnregisterFromEvents()
        {
            __broker?.RemoveHandler<LocalizationChangedArgs>(this);
        }

        /// <inheritdoc />
        public void OnEvent(LocalizationChangedArgs args)
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshProUGUI>();
                bufferedStringID = textMesh.text;
            }

            string localized = localizationManager.GetLocalized(bufferedStringID);
            if (localized == null)
                return;
            textMesh.text = localized;
            // recalculate button size if parent is an aci button
            AciButton button = textMesh.gameObject.GetComponentInParent<AciButton>();
            if (button == null)
                return;
            button.RecalculateLayout();
        }

        // Use this for initialization
        private void Start()
        {
            OnEvent(new LocalizationChangedArgs());
        }
    }
}