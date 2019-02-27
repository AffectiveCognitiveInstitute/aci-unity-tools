// <copyright file=HidePropertyIfDrawer.cs/>
// <copyright>
//   Copyright (c) 2019, Affective & Cognitive Institute
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
// <date>02/12/2019 09:43</date>
using Aci.Unity.Logging;
using UnityEditor;
using UnityEngine;

namespace Aci.Unity.Util
{
    /// <summary>
    /// Custom Property drawer used for objects with <see cref="HidePropertyIfAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(HidePropertyIfAttribute))]
    public class HidePropertyIfDrawer : PropertyDrawer
    {
        private HidePropertyIfAttribute m_Attr;
        private SerializedProperty m_Property;
        private bool m_Hidden = false;

        private void EvaluateVisibility(SerializedProperty property)
        {
            string path = property.propertyPath.Contains(".")
                ? System.IO.Path.ChangeExtension(property.propertyPath, m_Attr.conditionName)
                : m_Attr.conditionName;
            m_Property = property.serializedObject.FindProperty(path);
            switch (m_Property.type)
            {
                case "bool":
                    m_Hidden = m_Property.boolValue.Equals(m_Attr.conditionValue);
                    return;
                case "int":
                    m_Hidden = m_Property.intValue.Equals(m_Attr.conditionValue);
                    return;
                case "long":
                    m_Hidden = m_Property.longValue.Equals(m_Attr.conditionValue);
                    return;
                case "float":
                    m_Hidden = m_Property.floatValue.Equals(m_Attr.conditionValue);
                    return;
                case "double":
                    m_Hidden = m_Property.doubleValue.Equals(m_Attr.conditionValue);
                    return;
                case "Enum":
                    m_Hidden = m_Property.enumValueIndex.Equals((int) m_Attr.conditionValue);
                    return;
                case "string":
                    m_Hidden = m_Property.stringValue.Equals(m_Attr.conditionValue);
                    return;
                case "Vector2":
                    m_Hidden = m_Property.vector2Value.Equals(m_Attr.conditionValue);
                    return;
                case "Vector3":
                    m_Hidden = m_Property.vector3Value.Equals(m_Attr.conditionValue);
                    return;
                default:
                    AciLog.LogError("HidePropertyIfDrawer",
                                    $"Trying to compare a property {m_Property.name} of type {m_Property.type} failed.");
                    m_Hidden = false;
                    return;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (m_Attr == null)
                m_Attr = (HidePropertyIfAttribute)attribute;

            EvaluateVisibility(property);

            if (m_Hidden && m_Attr.visibilityBehaviour == HidePropertyIfAttribute.Visibility.Hide)
                return 0f;

            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_Attr == null)
                m_Attr = (HidePropertyIfAttribute)attribute;

            EvaluateVisibility(property);
            // If the condition is met, simply draw the field.
            if (!m_Hidden)
            {
                EditorGUI.PropertyField(position, property);
            }
            else if (m_Attr.visibilityBehaviour == HidePropertyIfAttribute.Visibility.Lock)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property);
                GUI.enabled = true;
            }
        }
    }
}