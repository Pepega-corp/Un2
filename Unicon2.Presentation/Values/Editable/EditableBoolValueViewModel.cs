using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values.Editable
{
    public class EditableBoolValueViewModel : EditableValueViewModelBase<IBoolValue>, IBoolValueViewModel
    {
        private IBoolValue _boolValue;
        private bool _boolValueProperty;
        private bool _baseValueBool;

        public EditableBoolValueViewModel()
        {
            IsEditEnabled = true;
        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.BOOL_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override void InitFromValue(IBoolValue value)
        {
            _baseValueBool = value.BoolValueProperty;
            _boolValue = value;
        }

        public override IBoolValue GetValue()
        {
            _boolValue.BoolValueProperty = BoolValueProperty;
            return _boolValue;
        }
        

        public bool BoolValueProperty
        {
            get { return _boolValueProperty; }
            set
            {
                _boolValueProperty = value;
                SetIsChangedProperty(nameof(BoolValueProperty), _baseValueBool != value);
                RaisePropertyChanged();
            }
        }

    }
}