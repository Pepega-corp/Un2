using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Model.Elements;
using Unicon2.Fragments.Measuring.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Factories
{
    public class MeasuringElementViewModelFactory : IMeasuringElementViewModelFactory
    {
        private readonly ITypesContainer _container;

        public MeasuringElementViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IMeasuringElementViewModel CreateMeasuringElementViewModel(IMeasuringElement measuringElement, string gruopName)
        {
            IMeasuringElementViewModel measuringElementViewModel =
                this._container.Resolve<IMeasuringElementViewModel>(measuringElement.StrongName +
                                                               ApplicationGlobalNames.CommonInjectionStrings
                                                                   .VIEW_MODEL);
            switch (measuringElement)
            {
	            case AnalogMeasuringElement analogMeasuringElement:
		            return CreateAnalogMeasuringElementViewModel(analogMeasuringElement);
	            case ControlSignal controlSignal:
					return CreateControlSignalViewModelViewModel(controlSignal);
				case DescretMeasuringElement descretMeasuringElement:
					return CreateDiscretMeasuringElementViewModel(descretMeasuringElement);
			}

            measuringElementViewModel.GroupName = gruopName;
            return measuringElementViewModel;
        }

        private AnalogMeasuringElementViewModel CreateAnalogMeasuringElementViewModel(
	        AnalogMeasuringElement analogMeasuringElement)
        {
			return new AnalogMeasuringElementViewModel();
        }

        private ControlSignalViewModel CreateControlSignalViewModelViewModel(
	        ControlSignal controlSignal)
        {
			return new ControlSignalViewModel();
        }

        private DiscretMeasuringElementViewModel CreateDiscretMeasuringElementViewModel(
	        DescretMeasuringElement discretMeasuringElement)
        {
			return new DiscretMeasuringElementViewModel();
        }
	}
}