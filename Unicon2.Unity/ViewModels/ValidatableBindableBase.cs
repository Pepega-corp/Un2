using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentValidation.Results;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Infrastructure.Common
{
    public abstract class ValidatableBindableBase : DisposableBindableBase, INotifyDataErrorInfo
    {

        private readonly Dictionary<string, List<ValidationFailure>> _errorDictionary = new Dictionary<string, List<ValidationFailure>>();

        public void FireErrorsChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            this?.OnValidate();
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;
            if (_errorDictionary.ContainsKey(propertyName))
            {
                return _errorDictionary[propertyName];
            }
            return null;
        }

        public void ClearErrors()
        {
            var props = _errorDictionary.Keys.ToList();
            _errorDictionary.Clear();
            foreach (var prop in props)
            {
                OnErrorsChanged(prop);
            }
        }

        public void SetValidationErrors(ValidationResult result)
        {
            _errorDictionary.Clear();
            foreach (ValidationFailure error in result.Errors)
            {
                if (_errorDictionary.ContainsKey(error.PropertyName))
                {
                    _errorDictionary[error.PropertyName].Add(error);
                }
                else
                {
                    _errorDictionary.Add(error.PropertyName, new List<ValidationFailure> { error });
                }
            }
        }

        public virtual void NotifyAll()
        {
            var props = _errorDictionary.Keys.ToList();
            foreach (var prop in props)
            {
                OnErrorsChanged(prop);
            }
        }

        public virtual bool HasErrors => _errorDictionary.Count != 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public virtual void Validate()
        {
            OnValidate();
        }
        protected virtual void OnValidate()
        {
           
        }
    }
}