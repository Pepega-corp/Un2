using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Presentation.Visitors
{
    public class EditableValueFetchingFromViewModelVisitor : IEditableValueViewModelVisitor<IFormattedValue>
    {
        public IFormattedValue VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IFormattedValue VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IFormattedValue VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IFormattedValue VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public IFormattedValue VisitStringValueViewModel(IStringValueViewModel stringValueViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}