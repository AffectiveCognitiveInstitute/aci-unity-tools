using System.Collections;
using UnityEditor;

namespace Aci.Unity.UI.Navigation
{
    [CustomEditor(typeof(ScreenController))]
    public class ScreenControllerEditor : Editor
    {
        private SerializedProperty m_PrefabLoadingStrategy;
        private SerializedProperty m_Prefab;
        private SerializedProperty m_PrefabPath;

        private void OnEnable()
        {
            m_PrefabLoadingStrategy = serializedObject.FindProperty("m_PrefabLoadingStrategy");
            m_Prefab = serializedObject.FindProperty("m_Prefab");
            m_PrefabPath = serializedObject.FindProperty("m_PrefabPath");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "m_Prefab", "m_PrefabPath");

            EditorGUILayout.PropertyField(m_PrefabLoadingStrategy.enumValueIndex == (int)ScreenController.PrefabLoadingStrategy.FromReference ?
                                            m_Prefab : m_PrefabPath);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

