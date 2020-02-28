using System;
using System.Collections.Generic;
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

        public void InitSubscription(IMemorySubscription memorySubscription)
        {
            _memorySubscription = memorySubscription;
        }

        public abstract T Accept<T>(IEditableValueViewModelVisitor<T> visitor);
        public IFormattedValue FormattedValue { get; set; }

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

        private readonly Lazy<Guid> _idLazy = new Lazy<Guid>(Guid.NewGuid);
        public Guid Id => _idLazy.Value;
    }
}