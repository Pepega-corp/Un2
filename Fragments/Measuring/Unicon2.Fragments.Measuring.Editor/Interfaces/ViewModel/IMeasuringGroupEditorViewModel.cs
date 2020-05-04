using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Editor.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel
{
    public interface IMeasuringGroupEditorViewModel : IStronglyNamed
    {
        string Header { get; set; }
        ObservableCollection<IMeasuringElementEditorViewModel> MeasuringElementEditorViewModels { get; set; }
        ICommand AddAnalogMeasuringElementCommand { get; }
        ICommand AddDiscretMeasuringElementCommand { get; }
        ICommand AddControlSignalCommand { get; }
        ICommand OpenPresentationSettingsCommand { get; }

        ICommand DeleteMeasuringElementCommand { get; }
        PresentationSettingsViewModel PresentationSettingsViewModel { get; set; }

    }
}