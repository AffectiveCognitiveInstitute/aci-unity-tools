﻿using UnityEngine;

namespace Aci.Unity.UI.Tweening
{
    public class PositionTweener : Tweener<Transform, Vector3>
    {
        protected override void ExecuteFrame(float percentage)
        {
            if (ReferenceEquals(m_Target, null) || m_Target == null)
                return;

            float t = m_Transition.Evaluate(percentage);
            m_Target.position = Vector3.LerpUnclamped(fromValue, toValue, t);
        }
    }
}