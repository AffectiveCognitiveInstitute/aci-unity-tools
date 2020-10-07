// <copyright file=AnchorTweener.cs/>
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
using UnityEngine;

namespace Aci.Unity.UI.Tweening
{
    public sealed class AnchorTweener : Tweener<RectTransform, RectTransformAnchor>
    {
        protected override void ExecuteFrame(float percentage)
        {
            if (ReferenceEquals(m_Target, null) || m_Target == null)
                return;

            float t = m_Transition.Evaluate(percentage);

            m_Target.anchorMin = Vector2.LerpUnclamped(m_FromValue.min, m_ToValue.min, t);
            m_Target.anchorMax = Vector2.LerpUnclamped(m_FromValue.max, m_ToValue.max, t);
        }

        protected override void Reset()
        {
            base.Reset();
            m_FromValue.min = m_Target.anchorMin;
            m_ToValue.max = m_Target.anchorMax;
        }
    }

    [Serializable]
    public struct RectTransformAnchor
    {
        [SerializeField] private Vector2 m_Min;
        [SerializeField] private Vector2 m_Max;

        public Vector2 min
        {
            get { return m_Min; }
            set { m_Min = value; }
        }

        public Vector2 max
        {
            get { return m_Max; }
            set { m_Max = value; }
        }
    }
}