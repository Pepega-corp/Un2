using MahApps.Metro.Controls.Dialogs;
using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Presentation.Values.Base;
using Unicon2.Presentation.Values.Validators;

namespace Unicon2.Presentation.Values.Editable
{
    public class EditableNumericValueViewModel : EditableValueViewModelBase, INumericValueViewModel
    {
        private string _numValueString;
        private readonly Lazy<double> _baseDoubleToCompareInitial;

        public EditableNumericValueViewModel()
        {
            _baseDoubleToCompareInitial = new Lazy<double>(() => double.Parse(NumValue));

        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.NUMERIC_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

      
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
                        Math.Abs(_baseDoubleToCompareInitial.Value - double.Parse(value)) > 0.0001);
                    RaisePropertyChanged();
                }
            }
        }

        protected override void OnValidate()
        {
            // FluentValidation.Results.ValidationResult res = new NumericValueViewModelValidator(_localizerService, this._ushortsFormatter).Validate(this);
            // SetValidationErrors(res);
        }
        
        public override T Accept<T>(IEditableValueViewModelVisitor<T> visitor)
        {
            return visitor.VisitNumericValueViewModel(this);
        }
    }
}






