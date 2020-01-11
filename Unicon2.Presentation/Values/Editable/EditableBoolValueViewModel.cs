using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values.Editable
{
    public class EditableBoolValueViewModel : EditableValueViewModelBase, IBoolValueViewModel
    {
        private IBoolValue _boolValue;
        private bool _boolValueProperty;
        private ushort[] _baseValueUshorts;
        private bool _baseValueBool;

        public EditableBoolValueViewModel()
        {
            this.IsEditEnabled = true;
        }


        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.BOOL_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override void InitFromValue(IFormattedValue value)
        {

            if (value is IBoolValue)
            {
                this._baseValueBool = (value as IBoolValue).BoolValueProperty;
                this.Model = value as IBoolValue;

            }
        }

        public override void SetBaseValueToCompare(ushort[] ushortsToCompare)
        {
            this._baseValueUshorts = ushortsToCompare;
            this._baseValueBool = (this._ushortsFormatter.Format(ushortsToCompare) as IBoolValue).BoolValueProperty;

            this.SetIsChangedProperty(nameof(this.BoolValueProperty), !this._baseValueUshorts.CheckEquality(this._boolValue.UshortsValue));

        }

        public override object Model
        {
            get { return this._boolValue; }
            set
            {
                this._boolValue = value as IBoolValue;
                this.BoolValueProperty = (value as IBoolValue).BoolValueProperty;
            }
        }

        public bool BoolValueProperty
        {
            get { return this._boolValueProperty; }
            set
            {
                this._boolValueProperty = value;
                if (this._boolValue != null)
                {
                    this._boolValue.BoolValueProperty = value;
                    this._boolValue.UshortsValue = this._ushortsFormatter?.FormatBack(this._boolValue);
                }
                this.SetIsChangedProperty(nameof(this.BoolValueProperty), this._baseValueBool != value);
                this.ValueChangedAction?.Invoke(this._boolValue.UshortsValue);
                this.RaisePropertyChanged();
                this.FormattedValueChanged?.Invoke(this, value);
            }
        }
    }
}
