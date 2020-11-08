using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel
{
    public interface IMeasuringMonitorEditorViewModel : IFragmentEditorViewModel
    {
        ICommand OpenConfigurationSettingsCommand { get; }
        ICommand AddMeasuringGroupCommand { get; }
        ICommand SetElementLeftCommand { get; }
        ICommand SetElementRightCommand { get; }
        ICommand DeleteGroupCommand { get; }
        ICommand CheckElementsPositionCommand { get; set; }
        ObservableCollection<IMeasuringGroupEditorViewModel> MeasuringGroupEditorViewModels { get; set; }
    }
}