using System.Windows.Input;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.FileOperations.ViewModel
{
    public class DeviceFileViewModel : BrowserElementViewModel, IDeviceFileViewModel
    {
        private IDeviceBrowserElement _model;
        private string _elementPath;
        private string _name;

        public DeviceFileViewModel()
        {
            this.DownloadElementCommand = new RelayCommand(this.OnDownloadElementExecute);
            this.ParentDeviceDirectoryViewModel?.LoadDirectoryCommand?.Execute(null);
        }

        private void OnDownloadElementExecute()
        {
            (this._model as IDeviceFile)?.Download();
        }


        public override object Model
        {
            get { return this._model; }
            set
            {
                this._model = value as IDeviceBrowserElement;
                this._elementPath = this._model.ElementPath;
                this._name = this._model.Name;
                this.RaisePropertyChanged(nameof(this.Name));

                this.RaisePropertyChanged(nameof(this.ElementPath));

            }
        }

        public override string ElementPath
        {
            get { return this._elementPath; }
        }

        public override string Name
        {
            get { return this._name; }
        }

        public override string StrongName => FileOperationsKeys.DEVICE_FILE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public ICommand DownloadElementCommand { get; }
    }
}
