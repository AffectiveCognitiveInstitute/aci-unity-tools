using UnityEngine;

namespace Aci.UI.Binding
{
    [CreateAssetMenu(menuName = "ACI/Value Converters/Boolean To Color Converter")]
    public class BooleanToColorConverter : ScriptableObject, IValueConverter
    {
        [SerializeField]
        private Color m_TrueValue;

        [SerializeField]
        private Color m_FalseValue;

        public object Convert(object value)
        {
            bool bValue = (bool)value;
            return bValue ? m_TrueValue : m_FalseValue;
        }

        public object ConvertBack(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}