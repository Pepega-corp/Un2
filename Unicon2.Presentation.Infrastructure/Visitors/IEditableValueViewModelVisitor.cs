using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.Visitors
{
    public interface IEditableValueViewModelVisitor<T>
    {
        T VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel);
        T VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel);
        T VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel);
        T VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel);
        T VisitStringValueViewModel(IStringValueViewModel stringValueViewModel);

    }
}