using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;

namespace Unicon2.Fragments.Measuring.Editor.Helpers
{
	public class MeasuringElementSaver
	{
		private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel=StaticContainer.Container.Resolve<ISharedResourcesGlobalViewModel>();

		public IMeasuringElement SaveMeasuringElement(IMeasuringElementEditorViewModel measuringElementEditorViewModel)
		{
			switch (measuringElementEditorViewModel)
			{
				case IAnalogMeasuringElementEditorViewModel analogMeasuringElementEditorViewModel:
					return CreateAnalogMeasuringElement(analogMeasuringElementEditorViewModel);
				case IControlSignalEditorViewModel controlSignalEditorViewModel:
					return CreateControlSignal(controlSignalEditorViewModel);

				case IDiscretMeasuringElementEditorViewModel discretMeasuringElementEditorViewModel:
					return CreateDiscretMeasuringElement(discretMeasuringElementEditorViewModel);
			    case IDateTimeMeasuringEditorViewModel dateTimeMeasuringEditorViewModel:
			        return this.CreateDateTimeMeasuringElement(dateTimeMeasuringEditorViewModel);
            }
			throw new Exception();
		}

		private void InitDefaults(IMeasuringElementEditorViewModel measuringElementEditorViewModel, IMeasuringElement measuringElement)
		{
			measuringElement.Name= measuringElementEditorViewModel.Header;
			measuringElement.SetId(measuringElementEditorViewModel.Id);
			if (_sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesContainsViewModel(measuringElementEditorViewModel))
			{
				_sharedResourcesGlobalViewModel.AddResourceFromViewModel(measuringElementEditorViewModel, measuringElement);
			}

			foreach (var dependencyViewModel in measuringElementEditorViewModel.DependencyViewModels)
			{
				if(dependencyViewModel is BoolToAddressDependencyViewModel boolToAddressDependencyViewModel)
				{
					measuringElement.Dependencies.Add(GetBoolToAddressDependency(boolToAddressDependencyViewModel));
				}
			}
		}


		private IDependency GetBoolToAddressDependency(BoolToAddressDependencyViewModel boolToAddressDependencyViewModel)
		{
			var res = StaticContainer.Container.Resolve<IBoolToAddressDependency>();
			res.RelatedResourceName = boolToAddressDependencyViewModel.RelatedResourceName;
			res.ResultingAddressIfFalse = boolToAddressDependencyViewModel.ResultingAddressIfFalse;
			res.ResultingAddressIfTrue = boolToAddressDependencyViewModel.ResultingAddressIfTrue;

			return res;
		}

		private IAnalogMeasuringElement CreateAnalogMeasuringElement(
			IAnalogMeasuringElementEditorViewModel analogMeasuringElementEditorViewModel)
		{
			IAnalogMeasuringElement analogMeasuringElement =
				StaticContainer.Container.Resolve<IAnalogMeasuringElement>();
			analogMeasuringElement.Address = analogMeasuringElementEditorViewModel.Address;
			analogMeasuringElement.NumberOfPoints = analogMeasuringElementEditorViewModel.NumberOfPoints;
			analogMeasuringElement.MeasureUnit = analogMeasuringElementEditorViewModel.MeasureUnit;
			analogMeasuringElement.IsMeasureUnitEnabled = analogMeasuringElementEditorViewModel.IsMeasureUnitEnabled;
			InitDefaults(analogMeasuringElementEditorViewModel, analogMeasuringElement);
			if (analogMeasuringElementEditorViewModel.FormatterParametersViewModel != null)
			{
				analogMeasuringElement.UshortsFormatter = StaticContainer.Container.Resolve<ISaveFormatterService>()
					.CreateUshortsParametersFormatter(analogMeasuringElementEditorViewModel.FormatterParametersViewModel);
			}
			return analogMeasuringElement;
		}
		private IControlSignal CreateControlSignal(
			IControlSignalEditorViewModel controlSignalEditorViewModel)
		{
			IControlSignal controlSignal =
				StaticContainer.Container.Resolve<IControlSignal>();
			InitDefaults(controlSignalEditorViewModel, controlSignal);
			controlSignal.WritingValueContext = new WritingValueContextSaver().CreateWritingValueContext(controlSignalEditorViewModel.WritingValueContextViewModel);
			return controlSignal;
		}
		private IDiscretMeasuringElement CreateDiscretMeasuringElement(
			IDiscretMeasuringElementEditorViewModel discretMeasuringElementEditorViewModel)
		{
			IDiscretMeasuringElement discretMeasuringElement =
				StaticContainer.Container.Resolve<IDiscretMeasuringElement>();
			InitDefaults(discretMeasuringElementEditorViewModel, discretMeasuringElement);
			discretMeasuringElement.AddressOfBit = new BitAddressSaver().GetAddressOfBitFromEditor(discretMeasuringElementEditorViewModel.BitAddressEditorViewModel);

			return discretMeasuringElement;
		}
	    private IDateTimeMeasuringElement CreateDateTimeMeasuringElement(
	        IDateTimeMeasuringEditorViewModel dateTimeMeasuringEditorViewModel)
	    {
	        IDateTimeMeasuringElement dateTimeMeasuringElement =
	            StaticContainer.Container.Resolve<IDateTimeMeasuringElement>();
	        InitDefaults(dateTimeMeasuringEditorViewModel, dateTimeMeasuringElement);
	        dateTimeMeasuringElement.StartAddress = dateTimeMeasuringEditorViewModel.StartAddress;

	        return dateTimeMeasuringElement;
	    }
    }
}
