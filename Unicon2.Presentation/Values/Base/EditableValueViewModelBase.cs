using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Values.Base
{
    public abstract class EditableValueViewModelBase<TFormattedValue> : FormattableValueViewModelBase<TFormattedValue>,
        IEditableValueViewModel<TFormattedValue>
    {

        public bool IsFormattedValueChanged => _signaturedIsChangedPropertyDictionary.ContainsValue(true);

        public abstract TFormattedValue GetValue();

        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set
            {
                _isEditEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void InitSubscription(IMemorySubscription memorySubscription)
        {
            _memorySubscription = memorySubscription;
        }

        private readonly Dictionary<string, bool> _signaturedIsChangedPropertyDictionary =
            new Dictionary<string, bool>();

        private bool _isEditEnabled;
        private IMemorySubscription _memorySubscription;

        protected void SetIsChangedProperty(string propertyName, bool isChanged)
        {
            if (!_signaturedIsChangedPropertyDictionary.ContainsKey(propertyName))
            {
                _signaturedIsChangedPropertyDictionary.Add(propertyName, isChanged);
            }
            else
            {
                _signaturedIsChangedPropertyDictionary[propertyName] = isChanged;
            }

            if (isChanged)
            {
                _memorySubscription?.Execute();
            }
            RaisePropertyChanged(nameof(IsFormattedValueChanged));
        }

        public Guid Id => Guid.NewGuid();
    }
}