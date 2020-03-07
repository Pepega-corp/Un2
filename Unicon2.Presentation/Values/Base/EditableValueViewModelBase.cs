using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Values.Base
{
    public abstract class EditableValueViewModelBase : FormattableValueViewModelBase,
        IEditableValueViewModel
    {

        public bool IsFormattedValueChanged => _signaturedIsChangedPropertyDictionary.ContainsValue(true);

        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set
            {
                _isEditEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void InitDispatcher(IDeviceEventsDispatcher deviceEventsDispatcher)
        {
            _deviceEventsDispatcher = deviceEventsDispatcher;
        }
        public abstract T Accept<T>(IEditableValueViewModelVisitor<T> visitor);
        public IFormattedValue FormattedValue { get; set; }

        public virtual void RefreshBaseValueToCompare()
        {
            foreach (var key in _signaturedIsChangedPropertyDictionary.Keys.ToList())
            {
                _signaturedIsChangedPropertyDictionary[key] = false;
            }

            RaisePropertyChanged(nameof(
                IsFormattedValueChanged
            ));
        }

        private readonly Dictionary<string, bool> _signaturedIsChangedPropertyDictionary =
            new Dictionary<string, bool>();

        private bool _isEditEnabled;
        private IDeviceEventsDispatcher _deviceEventsDispatcher;

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
                _deviceEventsDispatcher?.TriggerSubscriptionById(Id);
            }

            RaisePropertyChanged(nameof(IsFormattedValueChanged));
        }

        private readonly Lazy<Guid> _idLazy = new Lazy<Guid>(Guid.NewGuid);
        public Guid Id => _idLazy.Value;
    }
}