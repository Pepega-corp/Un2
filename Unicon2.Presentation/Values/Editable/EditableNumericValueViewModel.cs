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
        private double _baseDoubleToCompareInitial;

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
                        Math.Abs(_baseDoubleToCompareInitial - double.Parse(value)) > 0.0001);
                    RaisePropertyChanged();
                }
            }
        }
        public override void RefreshBaseValueToCompare()
        {
            _baseDoubleToCompareInitial = double.Parse(NumValue);
            base.RefreshBaseValueToCompare();
        }
        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult res = new NumericValueViewModelValidator().Validate(this);
            SetValidationErrors(res);
        }
        
        public override T Accept<T>(IEditableValueViewModelVisitor<T> visitor)
        {
            return visitor.VisitNumericValueViewModel(this);
        }
    }
}






