namespace Aci.UI.Binding
{
    public interface IValueConverter
    {
        object Convert(object value);
        object ConvertBack(object value);
    }
}
