using System.Windows.Input;

namespace Unicon2.Fragments.FileOperations.Infrastructure.ViewModel
{
    public interface INewDirectoryViewModel
    {
        string NewDirectoryPath { get; set; }
        string NewDirectoryName { get; set; }
        ICommand SubmitCommand { get; }
        ICommand CancelCommand { get; }
    }
}