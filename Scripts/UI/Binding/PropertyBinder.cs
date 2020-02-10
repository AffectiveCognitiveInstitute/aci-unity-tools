using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Aci.UI.Binding
{
    [DefaultExecutionOrder(-100)]
    public class PropertyBinder : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour m_BindingContext;

        [SerializeField]
        private Component m_TargetContext;

        [SerializeField]
        private UnityEngine.Object m_ValueConverter;

        [SerializeField]
        private string m_SourcePropertyName;

        [SerializeField]
        private string m_TargetPropertyName;

        [SerializeField]
        private UnityEvent m_BindingUpdated;

        private PropertyInfo m_SourcePropertyInfo;
        private PropertyInfo m_TargetPropertyInfo;

        private INotifyPropertyChanged m_NotifyPropertyChanged;

        private void Awake()
        {
            if (m_BindingContext == null)
                throw new MissingReferenceException("Missing reference to Binding Context");

            m_NotifyPropertyChanged = m_BindingContext as INotifyPropertyChanged;

            if (m_NotifyPropertyChanged == null)
                throw new InvalidCastException("BindingContext must implement INotifyPropertyChanged");

            m_SourcePropertyInfo = m_BindingContext.GetType().GetProperty(m_SourcePropertyName, BindingFlags.Public | BindingFlags.Instance);
            m_TargetPropertyInfo = m_TargetContext.GetType().GetProperty(m_TargetPropertyName, BindingFlags.Public | BindingFlags.Instance);
            SetDirty();
        }

        private void OnEnable()
        {
            m_NotifyPropertyChanged.propertyChanged += OnNotifyPropertyChanged;
            SetDirty();
        }

        private void OnDisable()
        {
            m_NotifyPropertyChanged.propertyChanged -= OnNotifyPropertyChanged;
        }

        private void OnNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.propertyName != m_SourcePropertyName)
                return;

            SetDirty();
        }

        private void SetDirty()
        {
            object value = m_SourcePropertyInfo.GetValue(m_BindingContext, null);

            if (m_ValueConverter != null)
            {
                IValueConverter converter = m_ValueConverter as IValueConverter;
                value = converter.Convert(value);
                m_TargetPropertyInfo.SetValue(m_TargetContext, value, null);
            }

            m_TargetPropertyInfo.SetValue(m_TargetContext, value, null);
            m_BindingUpdated?.Invoke();
        }
    }
}