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
    public class EditableNumericValueViewModel : EditableValueViewModelBase, INumericValueViewModel
    {
        private readonly ILocalizerService _localizerService;
        private INumericValue _numericValue;
        private string _numValueString;
        private double _baseDoubleToCompare;

        #region Overrides of EditableValueViewModelBase

        public EditableNumericValueViewModel(IDialogCoordinator dialogCoordinator, ILocalizerService localizerService)
        {
            this._localizerService = localizerService;
        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.NUMERIC_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public override void InitFromValue(IFormattedValue value)
        {
            this._baseDoubleToCompare = (value as INumericValue).NumValue;
            this.Model = value;
        }

        public override void SetBaseValueToCompare(ushort[] ushortsToCompare)
        {
            this._baseDoubleToCompare = (this._ushortsFormatter.Format(ushortsToCompare) as INumericValue).NumValue;
            this.SetIsChangedProperty(nameof(this.NumValue), Math.Abs(this._baseDoubleToCompare - this._numericValue.NumValue) > 0.5);
            //  SetIsChangedProperty(nameof(NumValue), !_baseValueToCompare.CheckEquality(_ushortsFormatter.FormatBack(_numericValue)));
        }

        public override object Model
        {
            get { return this._numericValue; }
            set
            {
                this._numericValue = value as INumericValue;
                this.NumValue = this._numericValue.NumValue.ToString();
            }
        }

        #endregion

        #region Implementation of INumericValueViewModel

        public string NumValue
        {
            get { return this._numValueString; }
            set
            {

                this._numValueString = value;
                this.FireErrorsChanged();
                if (!this.HasErrors)
                {
                    this._numericValue.NumValue = double.Parse(value);
                    this._numericValue.UshortsValue = this._ushortsFormatter.FormatBack(this._numericValue);
                    this.SetIsChangedProperty(nameof(this.NumValue), Math.Abs(this._baseDoubleToCompare - this._numericValue.NumValue) > 0.0001);

                    // SetIsChangedProperty(nameof(NumValue), !_baseValueToCompare.CheckEquality(_ushortsFormatter.FormatBack(_numericValue)));
                    this.ValueChangedAction?.Invoke(this._numericValue.UshortsValue);

                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Overrides of ValidatableBindableBase

        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult res = new NumericValueViewModelValidator(this._localizerService, this._ushortsFormatter).Validate(this);
            this.SetValidationErrors(res);
        }

        #endregion
    }






}
