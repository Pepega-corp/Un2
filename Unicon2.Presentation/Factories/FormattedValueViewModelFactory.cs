using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Visitors;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values;
using Unicon2.Unity.Annotations;

namespace Unicon2.Presentation.Factories
{
	public class FormattedValueViewModelFactory : IValueVisitor<IFormattedValueViewModel>
	{
		[CanBeNull] private IRangeable _rangeable;
		[CanBeNull] private IMeasurable _measurable;

		public FormattedValueViewModelFactory([CanBeNull] IRangeable rangeable = null, [CanBeNull] IMeasurable measurable = null)
		{
			_rangeable = rangeable;
			_measurable = measurable;
		}

		private IFormattedValueViewModel InitDefaults(IFormattedValue formattedValue,
			IFormattedValueViewModel formattedValueViewModel)
		{
			formattedValueViewModel.Header = formattedValue.Header;
			if (_measurable != null)
			{
				formattedValueViewModel.IsMeasureUnitEnabled = _measurable.IsMeasureUnitEnabled;
				formattedValueViewModel.MeasureUnit = _measurable.MeasureUnit;
			}
			else
			{
				formattedValueViewModel.IsMeasureUnitEnabled = false;
			}

			if (_rangeable != null)
			{
				formattedValueViewModel.IsRangeEnabled = _rangeable.IsRangeEnabled;
				formattedValueViewModel.Range = _rangeable.Range;
			}
			else
			{
				formattedValueViewModel.IsRangeEnabled = false;
			}

			return formattedValueViewModel;
		}

		private IFormattedValueViewModel GetDefaultStringValue(IFormattedValue formattedValue)
		{
			return InitDefaults(formattedValue, new StringValueViewModel() {StringValue = formattedValue.AsString()});
		}

		public IFormattedValueViewModel VisitBitMaskValue(IBitMaskValue bitMaskValue)
		{
			return InitDefaults(bitMaskValue,
				new BitMaskValueViewModel() {BitArray = bitMaskValue.BitArray});
		}

		public IFormattedValueViewModel VisitBoolValue(IBoolValue boolValue)
		{
			return InitDefaults(boolValue,
				new BoolValueViewModel() {BoolValueProperty = boolValue.BoolValueProperty});
		}

		public IFormattedValueViewModel VisitNumericValue(INumericValue numericValue)
		{
			return InitDefaults(numericValue,
				new NumericValueViewModel() {NumValue = numericValue.NumValue.ToString()});
		}

		public IFormattedValueViewModel VisitStringValue(IStringValue stringValue)
		{
			return InitDefaults(stringValue,
				new StringValueViewModel() {StringValue = stringValue.StrValue});
		}

		public IFormattedValueViewModel VisitTimeValue(ITimeValue timeValue)
		{
			return GetDefaultStringValue(timeValue);
		}

		public IFormattedValueViewModel VisitMatrixValue(IMatrixValue matrixValue)
		{
			return GetDefaultStringValue(matrixValue);
		}

		public IFormattedValueViewModel VisitChosenFromListValue(IChosenFromListValue chosenFromListValue)
		{
			return GetDefaultStringValue(chosenFromListValue);
		}

		public IFormattedValueViewModel VisitErrorValue(IErrorValue errorValue)
		{
			return GetDefaultStringValue(errorValue);
		}
	}
}