// <copyright file=TweenerEditor.cs/>
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
//   James Gay
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>08/01/2018 06:16</date>

using UnityEditor;
using UnityEngine;
using MyEditor = UnityEditor.Editor;

namespace Aci.Unity.UI.Tweening.Editor
{
    [CustomEditor(typeof(Tweener), true)]
    public class TweenerEditor : MyEditor
    {
        private static readonly Style s_Style = new Style();
        private                 float m_Time;

        public override void OnInspectorGUI()
        {
            Tweener target = (Tweener) this.target;
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            EditorGUI.BeginChangeCheck();
            m_Time = EditorGUILayout.Slider(s_Style.time, m_Time, 0f, 1f);
            if (EditorGUI.EndChangeCheck()) target.Seek(m_Time);

            EditorGUI.EndDisabledGroup();
        }

        public class Style
        {
            public GUIContent time = new GUIContent("Animation Preview");
        }
    }
}