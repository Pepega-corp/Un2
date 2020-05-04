using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Services;

namespace Unicon2.Fragments.Measuring.Editor.Helpers
{
	public class MeasuringElementSaver
	{
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
			}
			throw new Exception();
		}

		private void InitDefaults(IMeasuringElementEditorViewModel measuringElementEditorViewModel, IMeasuringElement measuringElement)
		{
			measuringElement.Name= measuringElementEditorViewModel.Header;
			measuringElement.SetId(measuringElementEditorViewModel.Id);
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
	}
}
