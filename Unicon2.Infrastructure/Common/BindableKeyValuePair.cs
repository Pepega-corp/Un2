using System;
using System.Collections;
using System.ComponentModel;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Infrastructure.Common
{
    public class BindableKeyValuePair<K, V> : ViewModelBase, INotifyDataErrorInfo
    {
        private K _key;
        private V _value;
        private bool _isInEditMode;
        public K Key
        {
            get { return _key; }
            set
            {
                _key = value;
                RaisePropertyChanged();
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Key)));

            }
        }

        public V Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Value)));

            }
        }

        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                RaisePropertyChanged();
            }
        }


        public BindableKeyValuePair(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public BindableKeyValuePair()
        {
            HasErrors = false;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasErrors { get; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}