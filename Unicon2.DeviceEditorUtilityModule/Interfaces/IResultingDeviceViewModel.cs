using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces
{
    public interface IResultingDeviceViewModel
    {
        ICommand NavigateToConnectionTestingCommand { get; }

        ObservableCollection<IFragmentEditorViewModel> AvailableEditorFragments { get; set; }
        void OpenSharedResources();
        ObservableCollection<IFragmentEditorViewModel> FragmentEditorViewModels { get; set; }
        void SaveDevice(string path, bool isDefaultSaving = true);
        void LoadDevice(string path);
        string DeviceName { get; set; }
    }
}