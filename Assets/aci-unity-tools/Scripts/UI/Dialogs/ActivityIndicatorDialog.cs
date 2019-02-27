using TMPro;
using UnityEngine;

namespace Aci.Unity.UI.Dialog
{
    public class ActivityIndicatorDialog : DialogBase
    {
        [SerializeField]
        private TextMeshProUGUI m_Text;

        public void Initialize(string text)
        {
            m_Text.text = text;
        }
    }
}

