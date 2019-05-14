using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements
{
    public interface IDeviceDirectoryViewModel : IBrowserElementViewModel
    {
        ObservableCollection<IBrowserElementViewModel> ChildBrowserElementViewModels { get; }
        ICommand LoadDirectoryCommand { get; }
        ICommand CreateChildDirectoryCommand { get; }
        ICommand UploadFileInDirectoryCommand { get; }

    }
}