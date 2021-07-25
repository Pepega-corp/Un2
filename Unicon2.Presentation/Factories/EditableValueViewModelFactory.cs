using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Visitors;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Unity.Properties;

namespace Unicon2.Presentation.Factories
{
	public class EditableValueViewModelFactory : IValueVisitor<IEditableValueViewModel>
	{
		private readonly bool _isEditingEnabled;
		[CanBeNull] private IRangeable _rangeable;
		[CanBeNull] private IMeasurable _measurable;

		public EditableValueViewModelFactory(bool isEditingEnabled,[CanBeNull] IRangeable rangeable = null, [CanBeNull] IMeasurable measurable = null)
		{
			_isEditingEnabled = isEditingEnabled;
			_rangeable = rangeable;
			_measurable = measurable;
		}

		private IEditableValueViewModel InitDefaults(IFormattedValue formattedValue,
			IEditableValueViewModel formattedValueViewModel)
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

			if (formattedValueViewModel.IsMeasureUnitEnabled)
			{

			}
		    formattedValueViewModel.FormattedValue = formattedValue;
		    formattedValueViewModel.IsEditEnabled = _isEditingEnabled;
			return formattedValueViewModel;
		}

		private IEditableValueViewModel GetDefaultStringValue(IFormattedValue formattedValue)
		{
			return InitDefaults(formattedValue, new StringValueViewModel() {StringValue = formattedValue.AsString()});
		}

		public IEditableValueViewModel VisitBitMaskValue(IBitMaskValue bitMaskValue)
		{
			return GetDefaultStringValue(bitMaskValue);
		}

		public IEditableValueViewModel VisitBoolValue(IBoolValue boolValue)
		{
			return InitDefaults(boolValue, new EditableBoolValueViewModel()
			{
				BoolValueProperty = boolValue.BoolValueProperty
			});
		}

		public IEditableValueViewModel VisitNumericValue(INumericValue numericValue)
		{
		    return InitDefaults(numericValue, new EditableNumericValueViewModel()
		    {
		        NumValue = numericValue.NumValue.ToString()
		    });
        }

		public IEditableValueViewModel VisitStringValue(IStringValue stringValue)
        {
            var res= InitDefaults(stringValue, new StringValueViewModel()
            {
                StringValue = stringValue.StrValue
            });

            res.IsEditEnabled = false;
            return res;
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
		    var res = new EditableChosenFromListValueViewModel()
		    {
		        SelectedItem = chosenFromListValue.SelectedItem
		    };
            res.InitList(chosenFromListValue.AvailableItemsList);
		    return InitDefaults(chosenFromListValue, res);
        }

		public IEditableValueViewModel VisitErrorValue(IErrorValue errorValue)
		{
			throw new NotImplementedException();
		}
	}
}