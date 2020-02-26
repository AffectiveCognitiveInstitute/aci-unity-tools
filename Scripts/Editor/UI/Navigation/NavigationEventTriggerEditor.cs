using System;
using UnityEditor;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    [CustomEditor(typeof(NavigationEventTrigger), true)]
    public class NavigationEventTriggerEditor : Editor
    {
        private const float ButtonWidth = 250.0f;
        private SerializedProperty m_DelegatesProperty;
        private GUIContent m_AddEventButtonContent;
        private GUIContent m_ToolbarMinusIcon;
        private GUIContent[] m_ButtonOptionsContent;

        protected virtual void OnEnable()
        {
            m_DelegatesProperty = serializedObject.FindProperty("m_Delegates");
            m_AddEventButtonContent = new GUIContent("Add new Navigation Event Trigger");
            m_ToolbarMinusIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            m_ToolbarMinusIcon.tooltip = "Remove all events in the list.";

            string[] triggerEventNames = Enum.GetNames(typeof(NavigationEventTrigger.EventType));
            m_ButtonOptionsContent = new GUIContent[triggerEventNames.Length];

            for (int i = 0; i < triggerEventNames.Length; i++)
                m_ButtonOptionsContent[i] = new GUIContent(triggerEventNames[i]);

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Vector2 iconDimensions = GUIStyle.none.CalcSize(m_ToolbarMinusIcon);
            int indexToRemove = -1;
            // Draw items
            for(int i = 0; i < m_DelegatesProperty.arraySize; i++)
            {
                SerializedProperty arrayElement = m_DelegatesProperty.GetArrayElementAtIndex(i);
                SerializedProperty eventTypeProperty = arrayElement.FindPropertyRelative("eventType");
                SerializedProperty navigationEventProperty = arrayElement.FindPropertyRelative("navigationEvent");
                EditorGUILayout.PropertyField(navigationEventProperty, m_ButtonOptionsContent[eventTypeProperty.enumValueIndex], new GUILayoutOption[0]);

                Rect lastRect = GUILayoutUtility.GetLastRect();
                if (GUI.Button(new Rect(lastRect.xMax - iconDimensions.x - 8.0f, lastRect.y + 1f, iconDimensions.x, iconDimensions.y), m_ToolbarMinusIcon, GUIStyle.none))
                    indexToRemove = i;

                EditorGUILayout.Space();
            }

            if (indexToRemove != -1)
                RemoveEntry(indexToRemove);

            // Draw button
            Rect rect = GUILayoutUtility.GetRect(m_AddEventButtonContent, GUI.skin.button);
            rect.x = rect.x +  ((rect.width - ButtonWidth) / 2.0f);
            rect.width = ButtonWidth;
            if(GUI.Button(rect, m_AddEventButtonContent, GUI.skin.button))
                DrawAddTriggerMenu();
            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveEntry(int indexToRemove)
        {
            m_DelegatesProperty.DeleteArrayElementAtIndex(indexToRemove);
        }

        private void DrawAddTriggerMenu()
        {
            GenericMenu menu = new GenericMenu();
            for(int i = 0; i < m_ButtonOptionsContent.Length; i++)
            {
                GUIContent content = m_ButtonOptionsContent[i];
                bool isAlreadyInList = false;
                // Test if event type is already in delegate list
                for(int j = 0; j < m_DelegatesProperty.arraySize; j++)
                {
                    if(m_DelegatesProperty.GetArrayElementAtIndex(j).FindPropertyRelative("eventType").enumValueIndex == i)
                    {
                        isAlreadyInList = true;
                        break;
                    }
                }

                if (isAlreadyInList)
                    menu.AddDisabledItem(content);
                else
                    menu.AddItem(content, false, new GenericMenu.MenuFunction2(OnTriggerMenuItemSelected), i);
            }

            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnTriggerMenuItemSelected(object userData)
        {
            int eventType = (int)userData;
            ++m_DelegatesProperty.arraySize;
            m_DelegatesProperty.GetArrayElementAtIndex(m_DelegatesProperty.arraySize - 1).FindPropertyRelative("eventType").enumValueIndex = eventType;
            serializedObject.ApplyModifiedProperties();
        }
    }
}