using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Presentation.Visitors
{
   public class EditableValueIsChangedVisitor: IEditableValueIsChangedVisitor
    {
        public bool VisitBoolValueViewModel(IBoolValueViewModel boolValueViewModel)
        {
            throw new NotImplementedException();
        }

        public bool VisitChosenFromListViewModel(IChosenFromListValueViewModel chosenFromListValueViewModel)
        {
            throw new NotImplementedException();
        }

        public bool VisitMatrixViewModel(IMatrixValueViewModel matrixValueViewModel)
        {
            throw new NotImplementedException();
        }

        public bool VisitNumericValueViewModel(INumericValueViewModel numericValueViewModel)
        {
            throw new NotImplementedException();
        }

        public bool VisitStringValueViewModel(IStringValueViewModel stringValueViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
