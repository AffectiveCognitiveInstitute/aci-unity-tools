using UnityEngine;

namespace Aci.UI.Binding
{
    [CreateAssetMenu(menuName = "ACI/Value Converters/Float To Color Converter")]
    public class FloatToColorConverter : ScriptableObject, IValueConverter
    {
        [SerializeField]
        private Gradient m_Gradient;

        public object Convert(object value)
        {
            float v = (float)value;
            return m_Gradient.Evaluate(v);
        }

        public object ConvertBack(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
