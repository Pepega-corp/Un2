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
            _container = container;
        }

        public IMeasuringElementViewModel CreateMeasuringElementViewModel(IMeasuringElement measuringElement, string groupName)
        {
            IMeasuringElementViewModel measuringElementViewModel =
                _container.Resolve<IMeasuringElementViewModel>(measuringElement.StrongName +
                                                               ApplicationGlobalNames.CommonInjectionStrings
                                                                   .VIEW_MODEL);
            switch (measuringElement)
            {
	            case AnalogMeasuringElement analogMeasuringElement:
		            return CreateAnalogMeasuringElementViewModel(analogMeasuringElement, groupName);
	            case ControlSignal controlSignal:
					return CreateControlSignalViewModelViewModel(controlSignal,groupName);
				case DescretMeasuringElement descretMeasuringElement:
					return CreateDiscretMeasuringElementViewModel(descretMeasuringElement, groupName);
                case DateTimeMeasuringElement dateTimeMeasuringElement:
                    return this.CreateDateTimeMeasuringElementViewModel(dateTimeMeasuringElement, groupName);
            }

            measuringElementViewModel.GroupName = groupName;
            return measuringElementViewModel;
        }

        private AnalogMeasuringElementViewModel CreateAnalogMeasuringElementViewModel(
	        AnalogMeasuringElement analogMeasuringElement, string groupName)
        {
			var res=new AnalogMeasuringElementViewModel();
			res.IsMeasureUnitEnabled = analogMeasuringElement.IsMeasureUnitEnabled;
			res.MeasureUnit = analogMeasuringElement.MeasureUnit;
			res.Id = analogMeasuringElement.Id;
			res.Header = analogMeasuringElement.Name;
			res.GroupName = groupName;
            return res;

        }

        private ControlSignalViewModel CreateControlSignalViewModelViewModel(
	        ControlSignal controlSignal, string groupName)
        {
	        var res = new ControlSignalViewModel();
	        res.Id = controlSignal.Id;
	        res.Header = controlSignal.Name.ToUpper();
	        res.GroupName = groupName;
	        return res;
        }

        private DiscretMeasuringElementViewModel CreateDiscretMeasuringElementViewModel(
	        DescretMeasuringElement discretMeasuringElement, string groupName)
        {
	        var res = new DiscretMeasuringElementViewModel();
	        res.Id = discretMeasuringElement.Id;
	        res.Header = discretMeasuringElement.Name;
	        res.GroupName = groupName;
            return res;
        }
        private DateTimeMeasuringElementViewModel CreateDateTimeMeasuringElementViewModel(
            DateTimeMeasuringElement dateTimeMeasuringElement, string groupName)
        {
            var res = new DateTimeMeasuringElementViewModel();
            res.Id = dateTimeMeasuringElement.Id;
            res.Header = dateTimeMeasuringElement.Name;
            res.GroupName = groupName;
            return res;
        }
    }
}