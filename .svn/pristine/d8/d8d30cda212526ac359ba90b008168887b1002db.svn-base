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
            get { return this._key; }
            set
            {
                this._key = value;
                this.RaisePropertyChanged();
            }
        }

        public V Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsInEditMode
        {
            get { return this._isInEditMode; }
            set
            {
                this._isInEditMode = value;
                this.RaisePropertyChanged();
            }
        }


        public BindableKeyValuePair(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }

        public BindableKeyValuePair()
        {
            this.HasErrors = false;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasErrors { get; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}