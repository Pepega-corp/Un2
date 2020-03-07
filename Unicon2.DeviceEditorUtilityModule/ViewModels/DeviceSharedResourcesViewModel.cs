using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class DeviceSharedResourcesViewModel : ViewModelBase, IDeviceSharedResourcesViewModel
    {
        private readonly Func<IResourceViewModel> _resourceViewModelGettingFunc;
        private readonly ISharedResourcesEditorFactory _sharedResourcesEditorFactory;
        private IResourceViewModel _selectedResourceViewModel;
        private bool _isSelectingMode;
        private ISerializerService _serializerService;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ITypesContainer _container;
        private bool _isInitialized;
        
        private string _lastPath;
        private string _lastFileName;
        private Type _typeNeeded;
        private const string DEFAULT_FOLDER = "SharedResources";
        private const string EXTENSION = ".sr";

        public DeviceSharedResourcesViewModel(Func<IResourceViewModel> resourceViewModelGettingFunc,
            ISharedResourcesEditorFactory sharedResourcesEditorFactory, ISerializerService serializerService,
            IApplicationGlobalCommands applicationGlobalCommands, ITypesContainer container)
        {
            ResourcesCollection = new ObservableCollection<IResourceViewModel>();
            _resourceViewModelGettingFunc = resourceViewModelGettingFunc;
            _sharedResourcesEditorFactory = sharedResourcesEditorFactory;
            CloseCommand = new RelayCommand<object>(OnCloseExecute);
            OpenResourceForEditingCommand = new RelayCommand<object>(OnOpenResourceForEditingExecute);
            SelectResourceCommand = new RelayCommand<object>(OnSelectExecute, CanExecuteSelectResource);
            DeleteResourceCommand = new RelayCommand(OnDeleteExecute, CanExecuteDeleteResource);
            RenameResourceCommand = new RelayCommand(OnRenameResourceExecute, CanExecuteRenameResource);

            _serializerService = serializerService;
            _applicationGlobalCommands = applicationGlobalCommands;
            _container = container;

            SaveCommand = new RelayCommand(SaveResources);
            LoadCommand = new RelayCommand(LoadResources);

            _lastPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
            _lastFileName = DEFAULT_FOLDER;
            if (_lastPath != null && !Directory.Exists(_lastPath))
            {
                Directory.CreateDirectory(_lastPath);
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
                InitialDirectory = _lastPath,
                FileName = Path.Combine(_lastPath, _lastFileName + EXTENSION)
            };
            if (dialog.ShowDialog() == true)
            {
                //_deviceSharedResources.SaveInFile(dialog.FileName, _serializerService);
                _lastPath = Path.GetDirectoryName(dialog.FileName);
                _lastFileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private void LoadResources()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = _lastPath,
                FileName = _lastFileName,
                Multiselect = false,
                Filter = $"Shared Resources (*{EXTENSION})|*{EXTENSION}",
                CheckFileExists = true
            };
            if (dialog.ShowDialog() == true)
            {
                // _deviceSharedResources.LoadFromFile(dialog.FileName, _serializerService);
                _lastPath = Path.GetDirectoryName(dialog.FileName);
                _lastFileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private bool CanExecuteRenameResource()
        {
            return SelectedResourceViewModel != null;
        }

        private void OnRenameResourceExecute()
        {
            SelectedResourceViewModel.StartEditElement();
        }

        private void OnSelectExecute(object obj)
        {
            OnCloseExecute(obj);
        }

        private bool CanExecuteDeleteResource()
        {
            return SelectedResourceViewModel != null;
        }

        private void OnDeleteExecute()
        {
            ResourcesCollection.Remove(SelectedResourceViewModel);
        }

        private bool CanExecuteSelectResource(object arg)
        {
            return SelectedResourceViewModel != null && SelectedResourceViewModel.GetType().GetInterfaces().Contains(_typeNeeded);
        }

        private bool CanExecuteOpenResourceForEditing(object _owner)
        {
            return SelectedResourceViewModel != null;
        }

        private void OnOpenResourceForEditingExecute(object _owner)
        {
            _sharedResourcesEditorFactory.OpenResourceForEdit(SelectedResourceViewModel, _owner);
        }

        private void OnCloseExecute(object obj)
        {
            (obj as Window)?.Close();
        }

        public string StrongName => nameof(DeviceSharedResourcesViewModel);

      
        private object GetModel()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<IResourceViewModel> ResourcesCollection { get; }

        public IResourceViewModel SelectedResourceViewModel
        {
            get { return _selectedResourceViewModel; }
            set
            {
                _selectedResourceViewModel?.StopEditElement();
                _selectedResourceViewModel = value;
                (OpenResourceForEditingCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (SelectResourceCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
                (DeleteResourceCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (RenameResourceCommand as RelayCommand)?.RaiseCanExecuteChanged();
                RaisePropertyChanged();
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
            get { return _isSelectingMode; }
            set
            {
                _isSelectingMode = value;
                RaisePropertyChanged();
            }
        }


        public void InitializeFromResources(IDeviceSharedResources deviceSharedResources)
        {
            ResourcesCollection.Clear();
            foreach (INameable sharedResource in deviceSharedResources.SharedResources)
            {
                IResourceViewModel resourceViewModel = _resourceViewModelGettingFunc();
             //   resourceViewModel.Model = sharedResource;
                ResourcesCollection.Add(resourceViewModel);
            }
            _isInitialized = true;
        }

        public void OpenSharedResourcesForEditing()
        {
            if (!_isInitialized) throw new Exception();
            IDeviceSharedResourcesViewModel deviceSharedResourcesViewModel = _container.Resolve<IDeviceSharedResourcesViewModel>();
          //  deviceSharedResourcesViewModel.Model = this._deviceSharedResources;
            deviceSharedResourcesViewModel.IsSelectingMode = false;
            _applicationGlobalCommands.ShowWindowModal(() => new DeviceSharedResourcesView(), deviceSharedResourcesViewModel);

        }

        public T OpenSharedResourcesForSelecting<T>()
        {
            if (!_isInitialized) throw new Exception();
            IsSelectingMode = true;
            _typeNeeded = typeof(T);
            _applicationGlobalCommands.ShowWindowModal((() => new DeviceSharedResourcesView()), this);
            if (SelectedResourceViewModel == null) return default(T);
            return (T)SelectedResourceViewModel;
        }

        public bool CheckDeviceSharedResourcesContainsElement(INameable resource)
        {
            return ResourcesCollection.Any(model =>model==resource);
        }

        public void AddSharedResource(INameable resourceToAdd)
        {
            if (!_isInitialized) throw new Exception();
            IResourcesAddingViewModel resourcesAddingViewModel = _container.Resolve<IResourcesAddingViewModel>();
            resourcesAddingViewModel.Model = resourceToAdd;
            //resourcesAddingViewModel.Initialize(_deviceSharedResources);
            _applicationGlobalCommands.ShowWindowModal(() => new ResourcesAddingWindow(), resourcesAddingViewModel);
        }
    }
}
