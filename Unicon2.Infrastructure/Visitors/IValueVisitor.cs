using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Infrastructure.Visitors
{
	public interface IValueVisitor<T>
	{
		T VisitBitMaskValue(IBitMaskValue bitMaskValue);
		T VisitBoolValue(IBoolValue boolValue);
		T VisitNumericValue(INumericValue numericValue);
		T VisitStringValue(IStringValue stringValue);
		T VisitTimeValue(ITimeValue timeValue);
		T VisitMatrixValue(IMatrixValue matrixValue);
		T VisitChosenFromListValue(IChosenFromListValue chosenFromListValue);
		T VisitErrorValue(IErrorValue errorValue);


	}
}