using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Visitors;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Factories
{
	public class EditableValueViewModelFactory: IValueVisitor<IEditableValueViewModel>
	{
		public IEditableValueViewModel VisitBitMaskValue(IBitMaskValue bitMaskValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitBoolValue(IBoolValue boolValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitNumericValue(INumericValue numericValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitStringValue(IStringValue stringValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitTimeValue(ITimeValue timeValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitMatrixValue(IMatrixValue matrixValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitChosenFromListValue(IChosenFromListValue chosenFromListValue)
		{
			throw new NotImplementedException();
		}

		public IEditableValueViewModel VisitErrorValue(IErrorValue errorValue)
		{
			throw new NotImplementedException();
		}
	}
}
