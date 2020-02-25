using MahApps.Metro.Controls.Dialogs;
using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;
using Unicon2.Presentation.Values.Validators;

namespace Unicon2.Presentation.Values.Editable
{
    public class EditableNumericValueViewModel : EditableValueViewModelBase<INumericValue>, INumericValueViewModel
    {
        private INumericValue _numericValue;
        private string _numValueString;
        private double _baseDoubleToCompare;

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.NUMERIC_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override void InitFromValue(INumericValue value)
        {
            _numericValue = value;
            _baseDoubleToCompare = _numericValue.NumValue;
            NumValue = _numericValue.NumValue.ToString();
        }

        public string NumValue
        {
            get { return _numValueString; }
            set
            {

                _numValueString = value;
                FireErrorsChanged();
                if (!HasErrors)
                {
                    SetIsChangedProperty(nameof(NumValue),
                        Math.Abs(_baseDoubleToCompare - double.Parse(value)) > 0.0001);
                    RaisePropertyChanged();
                }
            }
        }

        protected override void OnValidate()
        {
            // FluentValidation.Results.ValidationResult res = new NumericValueViewModelValidator(_localizerService, this._ushortsFormatter).Validate(this);
            // SetValidationErrors(res);
        }

        public override INumericValue GetValue()
        {
            if (!HasErrors)
            {
                _numericValue.NumValue = _numericValue.NumValue = double.Parse(NumValue);
            }

            return _numericValue;

        }
    }
}






