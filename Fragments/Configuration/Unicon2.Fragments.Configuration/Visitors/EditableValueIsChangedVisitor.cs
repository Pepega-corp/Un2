using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.Visitors
{
    public class EditableValueIsChangedVisitor:IEditableValueIsChangedVisitor
    {
        private readonly IFormattedValueViewModel _baseFormattedValueViewModel;

        public EditableValueIsChangedVisitor(IFormattedValueViewModel baseFormattedValueViewModel)
        {
            _baseFormattedValueViewModel = baseFormattedValueViewModel;
        }
        public bool VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel)
        {
            return (_baseFormattedValueViewModel as IBoolValueViewModel).BoolValueProperty !=
                   boolValueViewModel.BoolValueProperty;
        }

        public bool VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel)
        {
            return (_baseFormattedValueViewModel as IChosenFromListValueViewModel).SelectedItem !=
                   chosenFromListValueViewModel.SelectedItem;
        }

        public bool VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel)
        {
            throw new System.NotImplementedException();
        }

        public bool VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel)
        {
            return (_baseFormattedValueViewModel as INumericValueViewModel).NumValue !=
                   numericValueViewModel.NumValue;
        }

        public bool VisitStringValueViewModel(IStringValueViewModel stringValueViewModel)
        {
            return (_baseFormattedValueViewModel as IStringValueViewModel).StringValue !=
                   stringValueViewModel.StringValue;
        }
    }
}
