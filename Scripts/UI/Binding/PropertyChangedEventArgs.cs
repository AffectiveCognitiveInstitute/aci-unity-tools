namespace Aci.UI.Binding
{
    public struct PropertyChangedEventArgs
    {
        public string propertyName { get; private set; }

        public PropertyChangedEventArgs(string propertyName)
        {
            this.propertyName = propertyName;
        }
    }
}
