using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class DeviceSharedResourcesViewModel : ViewModelBase, IDeviceSharedResourcesViewModel
    {
        private readonly Func<IResourceViewModel> _resourceViewModelGettingFunc;
        private readonly ISharedResourcesEditorFactory _sharedResourcesEditorFactory;
        private IResourceViewModel _selectedResourceViewModel;
        private bool _isSelectingMode;
        private IDeviceSharedResources _deviceSharedResources;
        private ISerializerService _serializerService;
        private Type _typeNeeded;

        private string _lastPath;
        private string _lastFileName;
        private const string DEFAULT_FOLDER = "SharedResources";
        private const string EXTENSION = ".sr";

        public DeviceSharedResourcesViewModel(Func<IResourceViewModel> resourceViewModelGettingFunc,
            ISharedResourcesEditorFactory sharedResourcesEditorFactory, ISerializerService serializerService)
        {
            this.ResourcesCollection = new ObservableCollection<IResourceViewModel>();
            this._resourceViewModelGettingFunc = resourceViewModelGettingFunc;
            this._sharedResourcesEditorFactory = sharedResourcesEditorFactory;
            this.CloseCommand = new RelayCommand<object>(this.OnCloseExecute);
            this.OpenResourceForEditingCommand = new RelayCommand<object>(this.OnOpenResourceForEditingExecute);
            this.SelectResourceCommand = new RelayCommand<object>(this.OnSelectExecute, this.CanExecuteSelectResource);
            this.DeleteResourceCommand = new RelayCommand(this.OnDeleteExecute, this.CanExecuteDeleteResource);
            this.RenameResourceCommand = new RelayCommand(this.OnRenameResourceExecute, this.CanExecuteRenameResource);

            this._serializerService = serializerService;

            this.SaveCommand = new RelayCommand(this.SaveResources);
            this.LoadCommand = new RelayCommand(this.LoadResources);

            this._lastPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
            this._lastFileName = DEFAULT_FOLDER;
            if (this._lastPath != null && !Directory.Exists(this._lastPath))
            {
                Directory.CreateDirectory(this._lastPath);
            }
            else
            {
                //Load resources
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }

        private void SaveResources()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = this._lastPath,
                FileName = Path.Combine(this._lastPath, this._lastFileName + EXTENSION)
            };
            if (dialog.ShowDialog() == true)
            {
                this._deviceSharedResources.SaveInFile(dialog.FileName, this._serializerService);
                this._lastPath = Path.GetDirectoryName(dialog.FileName);
                this._lastFileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private void LoadResources()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = this._lastPath,
                FileName = this._lastFileName,
                Multiselect = false,
                Filter = $"Shared Resources (*{EXTENSION})|*{EXTENSION}",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == true)
            {
                this._deviceSharedResources.LoadFromFile(dialog.FileName, this._serializerService);
                this.SetModel(this._deviceSharedResources);
                this._lastPath = Path.GetDirectoryName(dialog.FileName);
                this._lastFileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private bool CanExecuteRenameResource()
        {
            return this.SelectedResourceViewModel != null;
        }

        private void OnRenameResourceExecute()
        {
            this.SelectedResourceViewModel.StartEditElement();
        }

        private void OnSelectExecute(object obj)
        {
            this.OnCloseExecute(obj);
        }

        private bool CanExecuteDeleteResource()
        {
            return this.SelectedResourceViewModel != null;
        }

        private void OnDeleteExecute()
        {
            this._deviceSharedResources.DeleteResource(this.SelectedResourceViewModel.Model as INameable);
            this.SetModel(this._deviceSharedResources);
        }

        private bool CanExecuteSelectResource(object arg)
        {
            return this.SelectedResourceViewModel != null && this.SelectedResourceViewModel.Model.GetType().GetInterfaces().Contains(this._typeNeeded);
        }

        private bool CanExecuteOpenResourceForEditing(object _owner)
        {
            return this.SelectedResourceViewModel != null;
        }

        private void OnOpenResourceForEditingExecute(object _owner)
        {
            this._sharedResourcesEditorFactory.OpenResourceForEdit(this.SelectedResourceViewModel.Model as INameable, _owner);
        }

        private void OnCloseExecute(object obj)
        {
            (obj as Window)?.Close();
        }

        public string StrongName => nameof(DeviceSharedResourcesViewModel);

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            if (value is IDeviceSharedResources)
            {
                this._deviceSharedResources = (IDeviceSharedResources)value;
                this.ResourcesCollection.Clear();
                foreach (INameable sharedResource in this._deviceSharedResources.SharedResources)
                {
                    IResourceViewModel resourceViewModel = this._resourceViewModelGettingFunc();
                    resourceViewModel.Model = sharedResource;
                    this.ResourcesCollection.Add(resourceViewModel);
                }
            }
        }

        private object GetModel()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<IResourceViewModel> ResourcesCollection { get; }

        public IResourceViewModel SelectedResourceViewModel
        {
            get { return this._selectedResourceViewModel; }
            set
            {
                this._selectedResourceViewModel?.StopEditElement();
                this._selectedResourceViewModel = value;
                (this.OpenResourceForEditingCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.SelectResourceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
                (this.DeleteResourceCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.RenameResourceCommand as RelayCommand)?.RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
            }
        }

        public ICommand OpenResourceForEditingCommand { get; }

        public ICommand SelectResourceCommand { get; }

        public ICommand DeleteResourceCommand { get; }

        public ICommand SubmitCommand { get; }

        public ICommand CloseCommand { get; }

        public ICommand RenameResourceCommand { get; }

        public bool IsSelectingMode
        {
            get { return this._isSelectingMode; }
            set
            {
                this._isSelectingMode = value;
                this.RaisePropertyChanged();
            }
        }

        public void Initialize(Type typeNeeded)
        {
            this._typeNeeded = typeNeeded;
        }
    }
}
