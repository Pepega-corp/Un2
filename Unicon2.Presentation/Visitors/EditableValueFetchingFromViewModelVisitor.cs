using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Presentation.Visitors
{
    public class EditableValueFetchingFromViewModelVisitor : IEditableValueFetchingFromViewModelVisitor
    {
        public IFormattedValue VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel)
        {
			var boolValue = (boolValueViewModel as IEditableValueViewModel)?.FormattedValue as IBoolValue;
			boolValue.BoolValueProperty = boolValueViewModel.BoolValueProperty;
			return boolValue;
		}

        public IFormattedValue VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel)
        {
            var chosenFromListValue = (chosenFromListValueViewModel as IEditableValueViewModel)?.FormattedValue as IChosenFromListValue;
            chosenFromListValue.SelectedItem = chosenFromListValueViewModel.SelectedItem;
            return chosenFromListValue;
        }

        public IFormattedValue VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IFormattedValue VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel)
        {
            var numericValue =(numericValueViewModel as IEditableValueViewModel)?.FormattedValue as INumericValue;
            numericValue.NumValue = double.Parse(numericValueViewModel.NumValue);
            return numericValue;
        }

        public IFormattedValue VisitStringValueViewModel(IStringValueViewModel stringValueViewModel)
        {
            var numericValue = (stringValueViewModel as IStringValueViewModel)?.FormattedValue as IStringValue;
            numericValue.StrValue = stringValueViewModel.StringValue;
            return numericValue;
        }
    }
}