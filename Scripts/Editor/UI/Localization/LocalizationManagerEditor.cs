// <copyright file=LocalizationManagerEditor.cs/>
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

using Aci.Unity.UI.Localization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{
    private GUIContent         localizationLabel;
    private GUIContent         patternLabel;
    private SerializedProperty baseLocalization;
    private SerializedProperty supportedPatterns;
    private SerializedProperty selectedPattern;
    private SerializedProperty selectedLocalization;

    private void OnEnable()
    {
        baseLocalization = serializedObject.FindProperty("baseLocalization");
        supportedPatterns = serializedObject.FindProperty("supportedReplacementPatterns");
        selectedPattern = serializedObject.FindProperty("_replacementPattern");
        selectedLocalization = serializedObject.FindProperty("_currentLocalization");

        localizationLabel = new GUIContent("Localized String Data");
        patternLabel = new GUIContent("Supported Replacement Patterns");
    }

    private void OnIETFChanged(object ietf)
    {
        serializedObject.Update();

        LocalizationManager manager = (LocalizationManager)target;

        selectedLocalization.stringValue = ietf as string;

        serializedObject.ApplyModifiedProperties();
    }

    private void OnPatternChanged(object pattern)
    {
        serializedObject.Update();

        LocalizationManager manager = (LocalizationManager)target;
        selectedPattern.stringValue = pattern as string;

        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // supported replacement patterns
        EditorGUILayout.PropertyField(supportedPatterns, patternLabel, true);

        // get target for easier value checking
        LocalizationManager manager = (LocalizationManager)target;

        // replacement pattern dropdown menu
        GenericMenu replacementPatternMenu = new GenericMenu
        {
            allowDuplicateNames = false
        };

        string currentPattern = manager.replacementPattern;

        for (int i = 0; i < manager.supportedReplacementPatterns.Length; ++i)
        {
            string newPattern = manager.supportedReplacementPatterns[i];
            replacementPatternMenu.AddItem(new GUIContent(newPattern), newPattern == currentPattern,
                                           OnPatternChanged, newPattern);
        }

        EditorGUILayout.BeginHorizontal(GUIStyle.none);
        EditorGUILayout.PrefixLabel("String Pattern");
        if (EditorGUILayout.DropdownButton(new GUIContent(currentPattern), FocusType.Keyboard))
            replacementPatternMenu.ShowAsContext();
        EditorGUILayout.EndHorizontal();

        // loaded data editor
        EditorGUILayout.PropertyField(baseLocalization, localizationLabel, true);


        // language drop down menu
        GenericMenu ietfmenu = new GenericMenu
        {
            allowDuplicateNames = false
        };

        if (manager.baseLocalization.Count == 0)
            selectedLocalization.stringValue = null;

        string selectedIetf = manager.currentLocalization;

        for (int i = 0; i < manager.baseLocalization.Count; ++i)
        {
            string ietf = manager.baseLocalization[i]?.languageIETF ?? "";
            ietfmenu.AddItem(new GUIContent(ietf), ietf == selectedIetf,
                             OnIETFChanged, ietf);
        }

        EditorGUILayout.BeginHorizontal(GUIStyle.none);
        EditorGUILayout.PrefixLabel("Target Language");
        if (EditorGUILayout.DropdownButton(new GUIContent(selectedIetf), FocusType.Keyboard)) ietfmenu.ShowAsContext();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}