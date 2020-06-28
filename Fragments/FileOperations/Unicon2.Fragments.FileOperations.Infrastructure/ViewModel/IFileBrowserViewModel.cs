using System.Windows.Input;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.ViewModel
{
    public interface IFileBrowserViewModel : IFragmentViewModel, IDeviceContextConsumer
    {
        IDeviceDirectoryViewModel RootDeviceDirectoryViewModel { get;}
        IDeviceDirectoryViewModel SelectedDirectoryViewModel { get; set; }
        ICommand SelectDirectoryCommand { get; }
        ICommand LoadRootCommand { get; }
    }
}