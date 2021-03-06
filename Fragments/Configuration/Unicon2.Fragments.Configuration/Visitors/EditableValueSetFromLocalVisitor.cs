﻿using System;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.Visitors
{
    public class EditableValueSetFromLocalVisitor : IEditableValueViewModelVisitor<Result>
    {
        private readonly IFormattedValue _formattedValue;

        public EditableValueSetFromLocalVisitor(IFormattedValue formattedValue)
        {
            _formattedValue = formattedValue;
        }
        public Result VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel)
        {
            boolValueViewModel.SetBoolValueProperty((_formattedValue as IBoolValue).BoolValueProperty);
            return Result.Create(true);
        }

        public Result VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel)
        {
            try
            {
  chosenFromListValueViewModel.SetValue((_formattedValue as IChosenFromListValue).SelectedItem);
            return Result.Create(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

        public Result VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel)
        {
            numericValueViewModel.NumValue = (_formattedValue as INumericValue).NumValue.ToString(); 
            return Result.Create(true);
        }

        public Result VisitStringValueViewModel(IStringValueViewModel stringValueViewModel)
        {
            stringValueViewModel.StringValue = (this._formattedValue as IStringValue).StrValue;
            return Result.Create(true);
        }
    }
}