using System.Windows.Input;

namespace Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements
{
    public interface IDeviceFileViewModel:IBrowserElementViewModel
    {
       ICommand DownloadElementCommand { get; }
    }
}