using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Aci.UI.Binding
{
    [CustomEditor(typeof(PropertyBinder))]
    public class PropertyBinderEditor : Editor
    {
        private SerializedProperty m_BindingContext;
        private SerializedProperty m_TargetContext;
        private SerializedProperty m_ValueConverter;
        private SerializedProperty m_SourcePropertyName;
        private SerializedProperty m_TargetPropertyName;
        private SerializedProperty m_BindingUpdated;
        private string[] m_BindingContextProperties;
        private int m_BindingContextPropertyIndex;

        private string[] m_TargetContextProperties;
        private int m_TargetContextPropertyIndex;
        private List<Entry> m_SourceEntries = new List<Entry>();
        private List<Entry> m_TargetEntries = new List<Entry>();

        private static class Style
        {
            public static GUIContent valueConverterStyle = new GUIContent("Value Converter (Optional)", "A value converter can be used to apply custom logic to bindings.");
        }

        private void OnEnable()
        {
            m_BindingContext = serializedObject.FindProperty("m_BindingContext");
            m_TargetContext = serializedObject.FindProperty("m_TargetContext");
            m_ValueConverter = serializedObject.FindProperty("m_ValueConverter");
            m_SourcePropertyName = serializedObject.FindProperty("m_SourcePropertyName");
            m_TargetPropertyName = serializedObject.FindProperty("m_TargetPropertyName");
            m_BindingUpdated = serializedObject.FindProperty("m_BindingUpdated");

            if (m_BindingContext.objectReferenceValue != null)
            {
                InvalidatePropertyValues(m_BindingContext.objectReferenceValue as MonoBehaviour, m_SourceEntries, ref m_BindingContextProperties, true);
                Entry e = m_SourceEntries.FirstOrDefault(x => x.component == m_BindingContext.objectReferenceValue && x.propertyName == m_SourcePropertyName.stringValue);
                m_BindingContextPropertyIndex = m_SourceEntries.IndexOf(e);
            }

            if (m_TargetContext.objectReferenceValue != null)
            {
                InvalidatePropertyValues(m_TargetContext.objectReferenceValue as MonoBehaviour, m_TargetEntries, ref m_TargetContextProperties, false);
                Entry e = m_TargetEntries.FirstOrDefault(x => x.component == m_TargetContext.objectReferenceValue && x.propertyName == m_TargetPropertyName.stringValue);
                m_TargetContextPropertyIndex = m_TargetEntries.IndexOf(e);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Validate();

            DrawContext(m_BindingContext, m_SourcePropertyName, ref m_BindingContextPropertyIndex, m_SourceEntries, ref m_BindingContextProperties, true);
            DrawContext(m_TargetContext, m_TargetPropertyName, ref m_TargetContextPropertyIndex, m_TargetEntries, ref m_TargetContextProperties, false);
            DrawValueConverter();
            EditorGUILayout.PropertyField(m_BindingUpdated);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawValueConverter()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ValueConverter, Style.valueConverterStyle);
            if(EditorGUI.EndChangeCheck())
            {
                if (m_ValueConverter.objectReferenceValue as IValueConverter == null)
                    m_ValueConverter.objectReferenceValue = null;
            }
        }

        private void DrawContext(SerializedProperty context, SerializedProperty property, ref int propertyIndex, List<Entry> entries, ref string [] propertyNames, bool onlyINotifyPropertyChanged)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(context);

            if (EditorGUI.EndChangeCheck())
            {
                InvalidatePropertyValues(context.objectReferenceValue as MonoBehaviour, entries, ref propertyNames, onlyINotifyPropertyChanged);
                propertyIndex = -1;
                if (entries.Count > 0)
                    context.objectReferenceValue = entries[0].component;
            }

            if (context.objectReferenceValue != null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(EditorGUIUtility.labelWidth);
                    try
                    {
                        propertyIndex = EditorGUILayout.Popup(propertyIndex, propertyNames);
                        if (propertyIndex != -1)
                        {
                            context.objectReferenceValue = entries[propertyIndex].component;
                            property.stringValue = entries[propertyIndex].propertyName;
                        }
                    }
                    catch (Exception) { /* Editor only, so ignore */}
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void InvalidatePropertyValues(MonoBehaviour component, List<Entry> entries, ref string[] propertyNames, bool onlyWithINotifyPropertyChanged)
        {
            entries.Clear();

            if (component == null)
            {
                propertyNames = new string[0];
                return;
            }

            MonoBehaviour[] components = null;
            if (onlyWithINotifyPropertyChanged)
                components = component.GetComponents<MonoBehaviour>().Where(x => (x as INotifyPropertyChanged) != null).ToArray();
            else
                components = component.GetComponents<MonoBehaviour>();

            for (int i = 0; i < components.Length; i++)
            {
                string[] properties = components[i].
                                      GetType().
                                      GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).
                                      Select(x => x.Name).ToArray();

                for (int j = 0; j < properties.Length; j++)
                {
                    Entry e = new Entry()
                    {
                        component = components[i],
                        propertyName = properties[j]
                    };
                    entries.Add(e);
                }
            }

            propertyNames = new string[entries.Count];
            for (int i = 0; i < entries.Count; i++)
                propertyNames[i] = entries[i].ToString();
        }

        private bool Validate()
        {
            if (m_BindingContext.objectReferenceValue != null)
            {
                INotifyPropertyChanged propertyChanged = m_BindingContext.objectReferenceValue as INotifyPropertyChanged;
                if (propertyChanged == null)
                {
                    EditorGUILayout.HelpBox("The Binding Context must implement INotifyPropertyChanged!", MessageType.Error);
                    return false;
                }
            }

            if (m_ValueConverter.objectReferenceValue != null)
            {
                IValueConverter valueConverter = m_ValueConverter.objectReferenceValue as IValueConverter;
                if (valueConverter == null)
                {
                    EditorGUILayout.HelpBox("The value converter must implement IValueConverter!", MessageType.Error);
                    return false;
                }
            }

            return true;
        }

        public struct Entry
        {
            public MonoBehaviour component;
            public string propertyName;

            public override string ToString()
            {
                return $"{component.GetType().Name}/{propertyName}";
            }
        }
    }
}

