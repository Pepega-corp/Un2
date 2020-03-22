using System;
using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Factories
{
	public class MeasuringElementEditorViewModelFactory : IMeasuringElementEditorViewModelFactory
	{
		private readonly ITypesContainer _container;
		private readonly IMeasuringElementFactory _measuringElementFactory;

		public MeasuringElementEditorViewModelFactory(ITypesContainer container,
			IMeasuringElementFactory measuringElementFactory)
		{
			_container = container;
			_measuringElementFactory = measuringElementFactory;
		}

		private void InitDefaults(IMeasuringElementEditorViewModel measuringElementEditorViewModel,
			IMeasuringElement measuringElement)
		{

			measuringElementEditorViewModel.Header = measuringElement.Name;
			measuringElementEditorViewModel.SetId(measuringElement.Id);
		}



		public IMeasuringElementEditorViewModel CreateMeasuringElementEditorViewModel(
			IMeasuringElement measuringElement)
		{
			switch (measuringElement)
			{
				case IAnalogMeasuringElement analogMeasuringElement:
					return CreateAnalogMeasuringElementEditorViewModel(analogMeasuringElement);
				case IControlSignal controlSignal:
					return CreateControlSignalEditorViewModel(controlSignal);
				case IDiscretMeasuringElement discretMeasuringElement:
					return CreateDiscretMeasuringElementEditorViewModel(discretMeasuringElement);
			}

			throw new Exception();

		}

		public IMeasuringElementEditorViewModel CreateAnalogMeasuringElementEditorViewModel(
			IAnalogMeasuringElement analogMeasuringElement = null)
		{
			if (analogMeasuringElement == null)
			{
				analogMeasuringElement = _measuringElementFactory.CreateAnalogMeasuringElement();
			}

			IAnalogMeasuringElementEditorViewModel analogMeasuringElementEditorViewModel =
				_container.Resolve<IMeasuringElementEditorViewModel>(MeasuringKeys.ANALOG_MEASURING_ELEMENT +
				                                                          ApplicationGlobalNames.CommonInjectionStrings
					                                                          .EDITOR_VIEWMODEL) as
					IAnalogMeasuringElementEditorViewModel;


			analogMeasuringElementEditorViewModel.Address = analogMeasuringElement.Address;
			analogMeasuringElementEditorViewModel.NumberOfPoints = analogMeasuringElement.NumberOfPoints;
			analogMeasuringElementEditorViewModel.MeasureUnit = analogMeasuringElement.MeasureUnit;
			analogMeasuringElementEditorViewModel.IsMeasureUnitEnabled = analogMeasuringElement.IsMeasureUnitEnabled;
			InitDefaults(analogMeasuringElementEditorViewModel, analogMeasuringElement);
			return analogMeasuringElementEditorViewModel;
		}

		public IMeasuringElementEditorViewModel CreateDiscretMeasuringElementEditorViewModel(
			IDiscretMeasuringElement discretMeasuringElement = null)
		{
			if (discretMeasuringElement == null)
			{
				discretMeasuringElement = _measuringElementFactory.CreateDiscretMeasuringElement();
			}

			IDiscretMeasuringElementEditorViewModel discretMeasuringElementEditorViewModel =
				_container.Resolve<IMeasuringElementEditorViewModel>(MeasuringKeys.DISCRET_MEASURING_ELEMENT +
				                                                          ApplicationGlobalNames.CommonInjectionStrings
					                                                          .EDITOR_VIEWMODEL) as
					IDiscretMeasuringElementEditorViewModel;

			discretMeasuringElementEditorViewModel.BitAddressEditorViewModel =
				new BitAddressEditorViewModelFactory().CreateBitAddressEditorViewModel(discretMeasuringElement
					.AddressOfBit);
			InitDefaults(discretMeasuringElementEditorViewModel, discretMeasuringElement);
			return discretMeasuringElementEditorViewModel;
		}

		public IMeasuringElementEditorViewModel CreateControlSignalEditorViewModel(IControlSignal controlSignal = null)
		{
			if (controlSignal == null)
			{
				controlSignal = _measuringElementFactory.CreateControlSignal();
			}

			IControlSignalEditorViewModel controlSignalEditorViewModel =
				_container.Resolve<IMeasuringElementEditorViewModel>(MeasuringKeys.CONTROL_SIGNAL +
				                                                          ApplicationGlobalNames.CommonInjectionStrings
					                                                          .EDITOR_VIEWMODEL) as
					IControlSignalEditorViewModel;

			controlSignalEditorViewModel.WritingValueContextViewModel =
				new WritingValueContextViewModelFactory().CreateWritingValueContextViewModel(controlSignal
					.WritingValueContext);
			InitDefaults(controlSignalEditorViewModel, controlSignal);
			return controlSignalEditorViewModel;
		}
	}
}