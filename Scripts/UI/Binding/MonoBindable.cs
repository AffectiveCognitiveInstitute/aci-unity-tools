using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Aci.UI.Binding
{
    public class MonoBindable : MonoBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedDelegate propertyChanged;

        public void SetProperty<T>(ref T property, T newValue, Action onChanged = null, [CallerMemberName] string propertyName = null)
        {
            if (property != null && EqualityComparer<T>.Default.Equals(property, newValue))
                return;

            property = newValue;
            RaisePropertyChanged(propertyName);
            onChanged?.Invoke();
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
