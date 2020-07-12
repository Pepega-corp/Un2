using System;
using System.Collections.Generic;
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
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.Properties;
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

		//private string _lastPath;
		//private string _lastFileName;
		private Type _typeNeeded;
		private IDeviceSharedResources _deviceSharedResources;
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

			//_lastPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
			//_lastFileName = DEFAULT_FOLDER;
			//if (_lastPath != null && !Directory.Exists(_lastPath))
		//	{
			//	Directory.CreateDirectory(_lastPath);
		//	}
		//	else
		//	{
		//		
		//	}
		}

		public ICommand SaveCommand { get; }
		public ICommand LoadCommand { get; }

		private void SaveResources()
		{
			SaveFileDialog dialog = new SaveFileDialog
			{
				Filter = $"Shared Resources (*{EXTENSION})|*{EXTENSION}",
			};
			if (dialog.ShowDialog() == true)
			{
				_serializerService.SerializeInFile(_deviceSharedResources.SharedResources,dialog.FileName);
			//	_lastPath = Path.GetDirectoryName(dialog.FileName);
			//	_lastFileName = Path.GetFileNameWithoutExtension(dialog.FileName);
			}
		}

		private void LoadResources()
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
	
				Multiselect = false,
				Filter = $"Shared Resources (*{EXTENSION})|*{EXTENSION}",
				CheckFileExists = true
			};
			if (dialog.ShowDialog() == true)
			{
				var resourcesFromFile = _serializerService.DeserializeFromFile<List<INameable>>(dialog.FileName);
				foreach (var resource in resourcesFromFile)
				{
					if (_deviceSharedResources.SharedResources.All(nameable => nameable.Name != resource.Name))
					{
						_deviceSharedResources.SharedResources.Add(resource);
					}
				}
				
				UpdateResourcesViewModelCollection();
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
			if (_resourceViewModelsInContainers.Contains(SelectedResourceViewModel))
			{
				_resourceViewModelsInContainers.Remove(SelectedResourceViewModel);
			}
			else
			{
				var sharedRes =
				_deviceSharedResources.SharedResources.FirstOrDefault(nameable =>
					nameable.Name == SelectedResourceViewModel.Name);
				if (sharedRes != null)
				{
					_deviceSharedResources.SharedResources.Remove(sharedRes);
				}
			}


			
			{
				
			}
			ResourcesCollection.Remove(SelectedResourceViewModel);

			UpdateResourcesViewModelCollection();
		}

		private bool CanExecuteSelectResource(object arg)
		{
			return SelectedResourceViewModel != null &&
			       SelectedResourceViewModel.RelatedEditorItemViewModel.GetType().GetInterfaces().Contains(_typeNeeded);
		}

		private void OnOpenResourceForEditingExecute(object _owner)
		{
			_sharedResourcesEditorFactory.OpenResourceForEdit(SelectedResourceViewModel, _owner);
		}

		private void OnCloseExecute(object obj)
		{
			(obj as Window)?.Close();
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
			_deviceSharedResources = deviceSharedResources;
			_isInitialized = true;
			ClearCaches();
		}

		public void OpenSharedResourcesForEditing()
		{
			if (!_isInitialized) throw new Exception();
			IsSelectingMode = false;
			UpdateResourcesViewModelCollection();
			_applicationGlobalCommands.ShowWindowModal(() => new DeviceSharedResourcesView(),
				this);
		}

		private void UpdateResourcesViewModelCollection()
		{
			var resourcesViewModels = _deviceSharedResources.SharedResources.Select(nameable =>
			{
				IResourceViewModel resourceViewModel = _resourceViewModelGettingFunc();
				resourceViewModel.RelatedEditorItemViewModel = nameable;
				resourceViewModel.Name = nameable.Name;
				return resourceViewModel;
			});
			ResourcesCollection.Clear();
			ResourcesCollection.AddCollection(_resourceViewModelsInContainers);
			ResourcesCollection.AddCollection(resourcesViewModels);
		}

		private List<IResourceViewModel> _resourceViewModelsInContainers = new List<IResourceViewModel>();

		public T OpenSharedResourcesForSelecting<T>()
		{
			if (!_isInitialized) throw new Exception();
			IsSelectingMode = true;
			UpdateResourcesViewModelCollection();
			_typeNeeded = typeof(T);
			(SelectResourceCommand as RelayCommand<object>).RaiseCanExecuteChanged();
			_applicationGlobalCommands.ShowWindowModal((() => new DeviceSharedResourcesView()), this);
			if (!CanExecuteSelectResource(null)) return default(T);
			return (T) SelectedResourceViewModel.RelatedEditorItemViewModel;
		}

		public string OpenSharedResourcesForSelectingString<T>()
		{
			if (!_isInitialized) throw new Exception();
			IsSelectingMode = true;
			_typeNeeded = typeof(T);
			UpdateResourcesViewModelCollection();
			(SelectResourceCommand as RelayCommand<object>).RaiseCanExecuteChanged();
			_applicationGlobalCommands.ShowWindowModal((() => new DeviceSharedResourcesView()), this);
			if (!CanExecuteSelectResource(null)) return String.Empty;
			return SelectedResourceViewModel.Name;
		}

		//public bool CheckDeviceSharedResourcesContainsModel(INameable resource)
		//{
		//          return _deviceSharedResources.SharedResources.Any(nameable => nameable == resource);
		//}

		public bool CheckDeviceSharedResourcesWithContainersContainsModel(object resource)
		{
			return _deviceSharedResources.SharedResourcesInContainers.Any(nameable => nameable.Resource == resource);
		}

		public bool CheckDeviceSharedResourcesContainsViewModel(string resourceName)
		{
			return ResourcesCollection.Any(model => model.Name == resourceName);
		}

		public bool CheckDeviceSharedResourcesContainsViewModel(object viewModel)
		{
			return ResourcesCollection.Any(model => model.RelatedEditorItemViewModel == viewModel);
		}


		public void AddAsSharedResource(INameable resourceModelToAdd)
		{
			if (!_isInitialized) throw new Exception();
			IResourcesAddingViewModel resourcesAddingViewModel = _container.Resolve<IResourcesAddingViewModel>();
			resourcesAddingViewModel.ResourceWithName = resourceModelToAdd;
			_applicationGlobalCommands.ShowWindowModal(() => new ResourcesAddingWindow(), resourcesAddingViewModel);
			if (resourcesAddingViewModel.IsResourceAdded)
			{
				_deviceSharedResources.SharedResources.Add(resourceModelToAdd);
			}
			UpdateResourcesViewModelCollection();
		}

		public void AddAsSharedResourceWithContainer(INameable resourceToAdd)
		{
			if (!_isInitialized) throw new Exception();
			IResourcesAddingViewModel resourcesAddingViewModel = _container.Resolve<IResourcesAddingViewModel>();
			IResourceViewModel resourceViewModel = _resourceViewModelGettingFunc();
			resourceViewModel.RelatedEditorItemViewModel = resourceToAdd;
			resourceViewModel.Name = resourceToAdd.Name;
			resourcesAddingViewModel.ResourceWithName = resourceViewModel;
			_applicationGlobalCommands.ShowWindowModal(() => new ResourcesAddingWindow(), resourcesAddingViewModel);
			if (resourcesAddingViewModel.IsResourceAdded)
			{
				_resourceViewModelsInContainers.Add(resourceViewModel);
			}
			UpdateResourcesViewModelCollection();
		}

		public void UpdateSharedResource(INameable resourceToAdd)
		{
			var existing=_deviceSharedResources.SharedResources.FirstOrDefault(nameable => nameable.Name == resourceToAdd.Name);
			if (existing!=null)
			{
				_deviceSharedResources.SharedResources.Remove(existing);
			}
			_deviceSharedResources.SharedResources.Add(resourceToAdd);
			UpdateResourcesViewModelCollection();
		}

		public IDeviceSharedResources GetSharedResources()
		{
			_deviceSharedResources.SharedResourcesInContainers = _resourcesWithContainers;
			return _deviceSharedResources;
		}

		public INameable GetResourceByName(string name)
		{
			return _deviceSharedResources.SharedResources.First(nameable => nameable.Name == name);
		}

		public void AddExistingResourceWithContainer(object viewModel, object resourceModel)
		{
			var resourceWithContainer =
				_deviceSharedResources.SharedResourcesInContainers.First(container =>
					container.Resource == resourceModel);
			_resourceViewModelsInContainers.Add(new ResourceViewModel()
			{
				Name = resourceWithContainer.ResourceName,
				RelatedEditorItemViewModel = viewModel as INameable
			});

		}

		private List<IResourceContainer> _resourcesWithContainers = new List<IResourceContainer>();

		public void AddResourceFromViewModel(object viewModel, object resourceModel)
		{
			var resourceWithContainer = _container.Resolve<IResourceContainer>();
			resourceWithContainer.Resource = resourceModel;
			resourceWithContainer.ResourceName =
				_resourceViewModelsInContainers.First(model => model.RelatedEditorItemViewModel == viewModel).Name;
			_resourcesWithContainers.Add(resourceWithContainer);
		}

		public void ClearCaches()
		{
			_resourcesWithContainers.Clear();
			_resourceViewModelsInContainers.Clear();
		}
	}
}