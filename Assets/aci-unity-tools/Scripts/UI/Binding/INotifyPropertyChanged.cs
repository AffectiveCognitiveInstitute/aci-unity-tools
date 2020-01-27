namespace Aci.UI.Binding
{
    public delegate void PropertyChangedDelegate(object sender, PropertyChangedEventArgs e); 

    public interface INotifyPropertyChanged
    {
        event PropertyChangedDelegate propertyChanged;
    }
}
