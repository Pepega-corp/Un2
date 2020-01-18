using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.FileOperations.ViewModel
{
    public class DeviceDirectoryViewModel : BrowserElementViewModel, IDeviceDirectoryViewModel
    {
        private readonly IBrowserElementViewModelFactory _browserElementViewModelFactory;
        private IDeviceDirectory _model;
        private string _elementPath;
        private string _name;

        public DeviceDirectoryViewModel(IBrowserElementViewModelFactory browserElementViewModelFactory)
        {
            this._browserElementViewModelFactory = browserElementViewModelFactory;
            this.ChildBrowserElementViewModels = new ObservableCollection<IBrowserElementViewModel>();
            this.CreateChildDirectoryCommand = new RelayCommand(this.OnCreateChildDirectoryExecute);
            this.UploadFileInDirectoryCommand = new RelayCommand(this.OnUploadFileInDirectoryExecute);
        }

        private void OnUploadFileInDirectoryExecute()
        {
            throw new NotImplementedException();
        }

        private void OnCreateChildDirectoryExecute()
        {
            throw new NotImplementedException();
        }


        public override string StrongName =>
            FileOperationsKeys.DEVICE_DIRECTORY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public override object Model
        {
            get
            {
                return this._model;

            }
            set
            {

                this._model = value as IDeviceDirectory;
                this._elementPath = this._model.ElementPath;
                this._name = this._model.Name;
                this.RaisePropertyChanged(nameof(this.ElementPath));
                this.RaisePropertyChanged(nameof(this.Name));

                this.ChildBrowserElementViewModels.Clear();
                this._model.BrowserElementsInDirectory?.ForEach((element =>
                    this.ChildBrowserElementViewModels.Add(
                        this._browserElementViewModelFactory.CreateBrowserElementViewModelBase(element, this))));

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

        public ObservableCollection<IBrowserElementViewModel> ChildBrowserElementViewModels { get; }

        public ICommand LoadDirectoryCommand { get; }

        public ICommand CreateChildDirectoryCommand { get; }

        public ICommand UploadFileInDirectoryCommand { get; }
    }
}
