using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.FileOperations.ViewModel
{
    public abstract class BrowserElementViewModel : ViewModelBase, IBrowserElementViewModel
    {
        private IDeviceDirectoryViewModel _parentDeviceDirectoryViewModel;

        public BrowserElementViewModel()
        {
            this.DeleteElementCommand = new RelayCommand(this.OnDeleteElementExecute);
        }

        private async void OnDeleteElementExecute()
        {
            Task<bool> deleteElementAsync = (this.Model as IDeviceBrowserElement)?.DeleteElementAsync();
            if (deleteElementAsync != null)
            {
                await deleteElementAsync;
            }
        }

        public abstract string StrongName { get; }

        public abstract object Model { get; set; }

        public IDeviceDirectoryViewModel ParentDeviceDirectoryViewModel
        {
            get { return this._parentDeviceDirectoryViewModel; }
            set
            {
                this._parentDeviceDirectoryViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand DeleteElementCommand { get; }

        public abstract string ElementPath { get; }
        public abstract string Name { get; }
    }
}
