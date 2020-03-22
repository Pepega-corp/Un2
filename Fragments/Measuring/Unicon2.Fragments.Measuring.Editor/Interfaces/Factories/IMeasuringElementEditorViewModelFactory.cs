using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.Factories
{
    public interface IMeasuringElementEditorViewModelFactory
    {
        IMeasuringElementEditorViewModel CreateMeasuringElementEditorViewModel(IMeasuringElement measuringElement);
        IMeasuringElementEditorViewModel CreateAnalogMeasuringElementEditorViewModel(IAnalogMeasuringElement analogMeasuringElement = null);
        IMeasuringElementEditorViewModel CreateDiscretMeasuringElementEditorViewModel(IDiscretMeasuringElement discretMeasuringElement = null);

        IMeasuringElementEditorViewModel CreateControlSignalEditorViewModel(IControlSignal controlSignal = null);

    }

}