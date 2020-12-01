using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values.Editable
{
    public class EditableBoolValueViewModel : EditableValueViewModelBase, IBoolValueViewModel
    {
        private bool _boolValueProperty;

        public EditableBoolValueViewModel()
        {
            IsEditEnabled = true;
        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.BOOL_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override string AsString()
        {
	        return BoolValueProperty.ToString();
        }


        public bool BoolValueProperty
        {
            get { return _boolValueProperty; }
            set
            {
                _boolValueProperty = value;
                SetIsChangedProperty();
                RaisePropertyChanged();
            }
        }

        public void SetBoolValueProperty(bool value)
        {
	        _boolValueProperty = value;
	        RaisePropertyChanged(nameof(BoolValueProperty));
		}


        public override T Accept<T>(IEditableValueViewModelVisitor<T> visitor)
        {
            return visitor.VisitBoolValueViewModel(this);
        }
    }
}