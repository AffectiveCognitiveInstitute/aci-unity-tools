﻿using UnityEngine;

namespace Aci.Unity.UI.Tweening
{
    public class AnchoredPositionTweener : Tweener<RectTransform, Vector2>
    {
        protected override void ExecuteFrame(float percentage)
        {
            float t = m_Transition.Evaluate(percentage);
            m_Target.anchoredPosition = Vector2.LerpUnclamped(m_FromValue, m_ToValue, t);
        }
    }
}