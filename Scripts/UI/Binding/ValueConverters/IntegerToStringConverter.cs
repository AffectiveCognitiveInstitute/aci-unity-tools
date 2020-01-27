using UnityEngine;

namespace Aci.UI.Binding
{
    [CreateAssetMenu(menuName = "ACI/Value Converters/Integer To String Converter")]
    public class IntegerToStringConverter : ScriptableObject, IValueConverter
    {
        public object Convert(object value)
        {
            return value.ToString();
        }

        public object ConvertBack(object value)
        {
            string sValue = (string)value;
            return int.Parse(sValue);
        }
    }
}
