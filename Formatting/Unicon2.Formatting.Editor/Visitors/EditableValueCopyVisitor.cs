using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Formatting.Editor.Visitors
{
    public class EditableValueCopyVisitor : IEditableValueCopyVisitor
    {
        private readonly IEditableValueViewModel _editableValueViewModel;

        public EditableValueCopyVisitor(IEditableValueViewModel editableValueViewModel)
        {
            _editableValueViewModel = editableValueViewModel;
        }

        public Result VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel)
        {
            var copyFrom = _editableValueViewModel as IBoolValueViewModel;
            if (copyFrom == null)
            {
                return Result.Create(false);
            }

            boolValueViewModel.BoolValueProperty = copyFrom.BoolValueProperty;

            return Result.Create(true);
        }

        public Result VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel)
        {
            var copyFrom = _editableValueViewModel as IChosenFromListValueViewModel;
            if (copyFrom == null)
            {
                return Result.Create(false);
            }

            chosenFromListValueViewModel.SelectedItem = copyFrom.SelectedItem;

            return Result.Create(true);
        }

        public Result VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel)
        {
            throw new NotImplementedException();
        }

        public Result VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel)
        {
            var copyFrom = _editableValueViewModel as INumericValueViewModel;
            if (copyFrom == null)
            {
                return Result.Create(false);
            }

            numericValueViewModel.NumValue = copyFrom.NumValue;

            return Result.Create(true);
        }

        public Result VisitStringValueViewModel(IStringValueViewModel stringValueViewModel)
        {
            throw new NotImplementedException();
        }
    }
}