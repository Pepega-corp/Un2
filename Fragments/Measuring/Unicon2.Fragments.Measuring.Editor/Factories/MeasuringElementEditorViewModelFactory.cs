using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
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
            this._container = container;
            this._measuringElementFactory = measuringElementFactory;
        }


        public IMeasuringElementEditorViewModel CreateMeasuringElementEditorViewModel(
            IMeasuringElement measuringElement)
        {
            IMeasuringElementEditorViewModel measuringElementEditorViewModel = this._container.Resolve<IMeasuringElementEditorViewModel>(measuringElement.StrongName +
                                                                  ApplicationGlobalNames.CommonInjectionStrings
                                                                      .EDITOR_VIEWMODEL);
            measuringElementEditorViewModel.Model = measuringElement;
            return measuringElementEditorViewModel;

        }

        public IMeasuringElementEditorViewModel CreateAnalogMeasuringElementEditorViewModel()
        {
            IMeasuringElement measuringElement = this._measuringElementFactory.CreateAnalogMeasuringElement();
            IAnalogMeasuringElementEditorViewModel analogMeasuringElementEditorViewModel =
                this._container.Resolve<IMeasuringElementEditorViewModel>(MeasuringKeys.ANALOG_MEASURING_ELEMENT +
                                                                     ApplicationGlobalNames.CommonInjectionStrings
                                                                         .EDITOR_VIEWMODEL) as
                    IAnalogMeasuringElementEditorViewModel;
            analogMeasuringElementEditorViewModel.Model = measuringElement;
            return analogMeasuringElementEditorViewModel;
        }

        public IMeasuringElementEditorViewModel CreateDiscretMeasuringElementEditorViewModel()
        {
            IMeasuringElement measuringElement = this._measuringElementFactory.CreateDiscretMeasuringElement();
            IDiscretMeasuringElementEditorViewModel discretMeasuringElementEditorViewModel =
                this._container.Resolve<IMeasuringElementEditorViewModel>(MeasuringKeys.DISCRET_MEASURING_ELEMENT +
                                                                     ApplicationGlobalNames.CommonInjectionStrings
                                                                         .EDITOR_VIEWMODEL) as
                    IDiscretMeasuringElementEditorViewModel;

            discretMeasuringElementEditorViewModel.Model = measuringElement;
            return discretMeasuringElementEditorViewModel;
        }

        public IMeasuringElementEditorViewModel CreateControlSignalEditorViewModel()
        {
            IMeasuringElement measuringElement = this._measuringElementFactory.CreateControlSignal();
            IControlSignalEditorViewModel controlSignalEditorViewModel =
                this._container.Resolve<IMeasuringElementEditorViewModel>(MeasuringKeys.CONTROL_SIGNAL +
                                                                     ApplicationGlobalNames.CommonInjectionStrings
                                                                         .EDITOR_VIEWMODEL) as
                    IControlSignalEditorViewModel;

            controlSignalEditorViewModel.Model = measuringElement;
            return controlSignalEditorViewModel;
        }
    }
}
