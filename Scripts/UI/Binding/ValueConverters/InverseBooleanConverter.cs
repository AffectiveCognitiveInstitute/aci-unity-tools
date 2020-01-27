using UnityEngine;

namespace Aci.UI.Binding
{
    [CreateAssetMenu(menuName = "ACI/Value Converters/Inverse Boolean Converter")]
    public class InverseBooleanConverter : ScriptableObject, IValueConverter
    {
        public object Convert(object value)
        {
            bool bValue = (bool)value;
            return !bValue;
        }

        public object ConvertBack(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
