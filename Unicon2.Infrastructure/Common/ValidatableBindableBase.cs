using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Infrastructure.Common
{
    public abstract class ValidatableBindableBase : DisposableBindableBase, INotifyDataErrorInfo
    {

        private readonly Dictionary<string, List<ValidationFailure>> _errorDictionary = new Dictionary<string, List<ValidationFailure>>();

        public void FireErrorsChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;
            if (this._errorDictionary.ContainsKey(propertyName))
            {
                return this._errorDictionary[propertyName];
            }
            return null;
        }

        public void SetValidationErrors(ValidationResult result)
        {
            this._errorDictionary.Clear();
            foreach (ValidationFailure error in result.Errors)
            {
                if (this._errorDictionary.ContainsKey(error.PropertyName))
                {
                    this._errorDictionary[error.PropertyName].Add(error);
                }
                else
                {
                    this._errorDictionary.Add(error.PropertyName, new List<ValidationFailure> { error });
                }
            }
        }

        public bool HasErrors => this._errorDictionary.Count != 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = OnErrorsChanged;

        private static void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            (sender as ValidatableBindableBase)?.OnValidate();
        }

        protected virtual void OnValidate()
        {
        }
    }
}