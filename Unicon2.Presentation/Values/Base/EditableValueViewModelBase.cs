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
	    public bool IsFormattedValueChanged
	    {
		    get => _isFormattedValueChanged;
		    set
		    {
			    _isFormattedValueChanged = value; 
				RaisePropertyChanged();
		    }
	    }

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
   

        private readonly Dictionary<string, bool> _signaturedIsChangedPropertyDictionary =
            new Dictionary<string, bool>();

        private bool _isEditEnabled;
        private IDeviceEventsDispatcher _deviceEventsDispatcher;

        protected void SetIsChangedProperty()
        {
	        _deviceEventsDispatcher?.TriggerSubscriptionById(Id);
        }

        private readonly Lazy<Guid> _idLazy = new Lazy<Guid>(Guid.NewGuid);
        private bool _isFormattedValueChanged;
        public Guid Id => _idLazy.Value;
    }
}