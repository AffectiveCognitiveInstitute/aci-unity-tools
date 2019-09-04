using UnityEditor;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    [CustomEditor(typeof(TweenerScreenTransition))]
    public class TweenerScreenTransitionEditor : Editor
    {
        private SerializedProperty m_UseSameTweenForReturnAndDestroy;
        private SerializedProperty m_EnterTween;
        private SerializedProperty m_ReturnTween;
        private SerializedProperty m_LeavingTween;
        private SerializedProperty m_DestroyTween;

        private void OnEnable()
        {
            m_UseSameTweenForReturnAndDestroy = serializedObject.FindProperty("m_UseSameTweenForReturnAndDestroy");
            m_EnterTween = serializedObject.FindProperty("m_EnterTween");
            m_ReturnTween = serializedObject.FindProperty("m_ReturnTween");
            m_LeavingTween = serializedObject.FindProperty("m_LeavingTween");
            m_DestroyTween = serializedObject.FindProperty("m_DestroyedTween");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.HelpBox("Check this box if you want to use the same animation when entering and returning, and also for leaving and destroying", MessageType.Info);

            EditorGUILayout.PropertyField(m_UseSameTweenForReturnAndDestroy, new GUIContent("Use same Tweens"));

            if (m_UseSameTweenForReturnAndDestroy.boolValue)
            {
                EditorGUILayout.PropertyField(m_EnterTween, true);
                EditorGUILayout.PropertyField(m_LeavingTween, true);
            }
            else
            {
                EditorGUILayout.PropertyField(m_EnterTween, true);
                EditorGUILayout.PropertyField(m_LeavingTween, true);
                EditorGUILayout.PropertyField(m_ReturnTween, true);
                EditorGUILayout.PropertyField(m_DestroyTween, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

