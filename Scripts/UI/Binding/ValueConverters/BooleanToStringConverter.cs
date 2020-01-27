using UnityEngine;

namespace Aci.UI.Binding
{
    [CreateAssetMenu(menuName = "ACI/Value Converters/Boolean To String Converter")]
    public class BooleanToStringConverter : ScriptableObject, IValueConverter
    {
        [SerializeField]
        private string m_TrueValue;

        [SerializeField]
        private string m_FalseValue;

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
