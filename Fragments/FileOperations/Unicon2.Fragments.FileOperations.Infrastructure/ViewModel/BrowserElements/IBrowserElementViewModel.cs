using System.Windows.Input;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements
{
    public interface IBrowserElementViewModel:IViewModel
    {
        IDeviceDirectoryViewModel ParentDeviceDirectoryViewModel { get; set; }
        ICommand DeleteElementCommand { get; }
        string ElementPath { get; }
        string Name { get; }
    }
}