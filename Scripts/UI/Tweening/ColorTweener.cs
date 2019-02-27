using UnityEngine;
using UnityEngine.UI;

namespace Aci.Unity.UI.Tweening
{
    public class ColorTweener : Tweener<Graphic, Color>
    {
        protected override void ExecuteFrame(float percentage)
        {
            m_Target.color = Color.Lerp(m_FromValue, m_ToValue, percentage);
        }
    }
}
