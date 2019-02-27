// <copyright file=LocalizationDataEditor.cs/>
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
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Aci.Unity.UI.Localization
{
    [CustomEditor(typeof(LocalizationData))]
    public class LocalizationDataEditor : Editor
    {
        private bool       bindingsShown = true;
        private GUIContent iconPause;
        private GUIStyle   iconStyle;

        private GUIContent iconToolbarMinus;
        private GUIContent iconToolbarPlus;

        private SerializedProperty internalStringData;

        private SerializedProperty languageDescriptor;
        private SerializedProperty languageIdentifier;

        private void OnEnable()
        {
            languageIdentifier = serializedObject.FindProperty("languageIETF");
            languageDescriptor = serializedObject.FindProperty("languageDescriptor");
            internalStringData = serializedObject.FindProperty("internalStringData");

            iconToolbarPlus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"));
            iconToolbarPlus.tooltip = "Add new localized string.";
            iconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            iconToolbarMinus.tooltip = "Remove this localized string.";
            iconPause = new GUIContent(EditorGUIUtility.IconContent("PauseButton"));
            iconPause.tooltip = "Drag to change order";

            Vector2 iconSizes = GUIStyle.none.CalcSize(iconToolbarPlus);
            iconStyle = new GUIStyle(GUIStyle.none)
            {
                fixedWidth = iconSizes.x,
                fixedHeight = iconSizes.y,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageOnly
            };
        }

        void ImportLocalizationJSON()
        {
            string file = EditorUtility.OpenFilePanelWithFilters("Import Localization from JSON...", Application.dataPath, new []{"JSON localization file","json" } );
            LocalizationData data = (LocalizationData)target;

            string url = "file://" + file;
            //check if file is actual json file
            if (!url.EndsWith(".json"))
                return;

            WWW targetFile = new WWW(url);
            while (!targetFile.isDone)
            {
            }

            string[] parts = url.Split(new[] { ".", "/" }, StringSplitOptions.None);
            CultureInfo info = CultureInfo.GetCultureInfoByIetfLanguageTag(parts[parts.Length - 2]);
            data.languageIETF = info.IetfLanguageTag;
            data.languageDescriptor = info.ThreeLetterISOLanguageName;
            data.FromJSON(targetFile.text);
        }
        void ExportLocalizationJSON()
        {
            string file = EditorUtility.SaveFilePanel("Export Localization to JSON...", Application.dataPath, target.name, "json");
            LocalizationData data = (LocalizationData)target;
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.Write(data.ToJSON());
            }
        }

        private void OnIetfChanged(object key)
        {
            serializedObject.Update();

            languageIdentifier.stringValue = key as string;
            languageDescriptor.stringValue = CultureInfo.GetCultureInfoByIetfLanguageTag(key as string).ThreeLetterISOLanguageName;

            serializedObject.ApplyModifiedProperties();
        }

        private void ManageIdentifierLocalizationBindings()
        {
            LocalizationData data = (LocalizationData) target;
            bindingsShown = EditorGUILayout.Foldout(bindingsShown, new GUIContent("Localized Strings"), true);

            if (!bindingsShown)
                return;

            SerializedProperty bufferProp = internalStringData.Copy();

            SerializedProperty keysProperty = null;
            SerializedProperty valuesProperty = null;

            SerializedProperty endProperty = bufferProp.GetEndProperty();
            while (bufferProp.NextVisible(true))
            {
                if (SerializedProperty.EqualContents(bufferProp, endProperty)) break;

                if (bufferProp.name == "keys")
                    keysProperty = bufferProp.Copy();

                if (bufferProp.name == "values")
                    valuesProperty = bufferProp.Copy();
            }

            Vector2 removeButtonSize = GUIStyle.none.CalcSize(iconToolbarMinus);
            Vector2 addButtonSize = GUIStyle.none.CalcSize(iconToolbarPlus);

            // Decorators
            EditorGUILayout.BeginHorizontal();

            float curWidth = EditorGUIUtility.currentViewWidth - 10 * 2 - removeButtonSize.x - removeButtonSize.x;

            GUILayout.Space(removeButtonSize.x + 2);
            GUILayout.Label("String Identifier", GUILayout.Width(curWidth * 0.5f - 5));
            GUILayout.Label("Localized String Value", GUILayout.Width(curWidth * 0.5f - 5));
            if (GUILayout.Button(iconToolbarPlus, iconStyle))
            {
                // new element here
                keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
                SerializedProperty newProp = keysProperty.GetArrayElementAtIndex(keysProperty.arraySize - 1);
                string newValue = "newValue";
                int count = 0;
                while (data.stringData.ContainsKey(newValue))
                    newValue = "newValue" + ++count;
                newProp.stringValue = newValue;
                valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
            }

            EditorGUILayout.EndHorizontal();

            // Elements
            int toBeRemovedEntry = -1;

            Rect stringsRect = EditorGUILayout.BeginVertical();

            for (int i = 0; i < keysProperty.arraySize; ++i)
            {
                Rect kvpRect = EditorGUILayout.BeginHorizontal();
                GUILayout.Button(iconPause, iconStyle);
                SerializedProperty keyProperty = keysProperty.GetArrayElementAtIndex(i);
                SerializedProperty valueProperty = valuesProperty.GetArrayElementAtIndex(i);
                string newValue = EditorGUILayout.TextField(keyProperty.stringValue, GUILayout.Width(curWidth * 0.5f - 5));
                if (!data.stringData.ContainsKey(newValue))
                    keyProperty.stringValue = newValue;
                valueProperty.stringValue =
                    EditorGUILayout.TextField(valueProperty.stringValue, GUILayout.Width(curWidth * 0.5f - 5));
                if (GUILayout.Button(iconToolbarMinus, iconStyle)) toBeRemovedEntry = i;
                EditorGUILayout.EndHorizontal();
            }

            // remove Elements
            if (toBeRemovedEntry > -1)
            {
                keysProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
                valuesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
            }

            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // export / import
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            if(GUILayout.Button("Export to JSON"))
                ExportLocalizationJSON();
            if(GUILayout.Button("Import from JSON"))
                ImportLocalizationJSON();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            // language toggle
            GenericMenu ietfmenu = new GenericMenu
            {
                allowDuplicateNames = false
            };

            string selectedIetf = languageIdentifier.stringValue;

            string selectedDesc = languageDescriptor.stringValue;

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            for (int i = 0; i < cultures.Length; ++i)
            {
                string key = cultures[i].IetfLanguageTag;
                ietfmenu.AddItem(new GUIContent(key), key == selectedIetf, OnIetfChanged, key);
            }

            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            EditorGUILayout.PrefixLabel("Target Language");
            if (EditorGUILayout.DropdownButton(new GUIContent(selectedIetf), FocusType.Keyboard))
                ietfmenu.ShowAsContext();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("ISO Language Descriptor", selectedDesc, "Textfield");

            EditorGUILayout.Separator();

            // bindings
            ManageIdentifierLocalizationBindings();

            serializedObject.ApplyModifiedProperties();
        }
    }
}