using MahApps.Metro.Controls.Dialogs;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Presentation.ViewModels;
using Unicon2.Shell.Factories;
using Unicon2.Shell.ViewModels.Helpers;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ILogServiceViewModel LogServiceViewModel { get; }
        public IProjectBrowserViewModel ProjectBrowserViewModel { get; }

        private readonly IDialogCoordinator _dialogCoordinator;

        private bool _isMenuFlyOutOpen;

        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly IRegionManager _regionManager;
        private readonly ILocalizerService _localizerService;
        private readonly IDeviceViewModelFactory _deviceViewModelFactory;
        private readonly IFragmentPaneViewModelFactory _fragmentPaneViewModelFactory;
        private readonly IUniconProjectService _uniconProjectService;
        private readonly IDevicesContainerService _devicesContainerService;
        private ObservableCollection<IFragmentPaneViewModel> _fragmentsOpenedCollection;
        private IFragmentPaneViewModel _activeFragmentViewModel;
        private RecentProjectsViewModelFactory _recentProjectsViewModelFactory;

        public ShellViewModel
        (ILogService logService,
            ILogServiceViewModel logServiceViewModel,
            IDevicesContainerService devicesContainerService,
            IDialogCoordinator dialogCoordinator,
            IApplicationGlobalCommands applicationGlobalCommands,
            IRegionManager regionManager,
            ILocalizerService localizerService,
            IDeviceViewModelFactory deviceViewModelFactory,
            IFragmentPaneViewModelFactory fragmentPaneViewModelFactory,
            IProjectBrowserViewModel projectBrowserViewModel,
            IUniconProjectService uniconProjectService, ToolBarViewModel toolBarViewModel, 
            RecentProjectsViewModelFactory recentProjectsViewModelFactory)
        {
            LogServiceViewModel = logServiceViewModel;
            ProjectBrowserViewModel = projectBrowserViewModel;
            LogService = logService;
            _devicesContainerService = devicesContainerService;
            _devicesContainerService.ConnectableItemChanged += OnDeviceChanged;
            _dialogCoordinator = dialogCoordinator;
            _applicationGlobalCommands = applicationGlobalCommands;
            _regionManager = regionManager;
            _localizerService = localizerService;
            _deviceViewModelFactory = deviceViewModelFactory;
            _fragmentPaneViewModelFactory = fragmentPaneViewModelFactory;
            _uniconProjectService = uniconProjectService;

            ExitCommand = new RelayCommand(OnExit);
            NavigateToDeviceEditorCommand = new RelayCommand(OnNavigateToDeviceEditor);
            NavigateToDeviceAddingCommand = new RelayCommand(OnNavigateToDeviceAddingExecute);
            NavigateToWebSyncViewCommand = new RelayCommand(OnNavigateToWebSyncViewExecute);
            AddNewFragmentCommand = new RelayCommand<IFragmentViewModel>(OnExecuteAddNewFragment);
            _fragmentsOpenedCollection = new ObservableCollection<IFragmentPaneViewModel>();
            OpenOscillogramCommand = new RelayCommand(OnOpenOscillogramExecute);
            OnClosingCommand = new RelayCommand<CancelEventArgs>(OnExecuteClosing);
            AnchorableWindows = new ObservableCollection<IAnchorableWindow>
            {
                ProjectBrowserViewModel, LogServiceViewModel
            };
            ToolBarViewModel = toolBarViewModel;
            _recentProjectsViewModelFactory = recentProjectsViewModelFactory;
            StaticOptionsButtonsHelper.InitializeStaticButtons(this);
            NewProjectCommand = new RelayCommand(OnNewProjectExecute);
            SaveProjectCommand = new RelayCommand(OnSaveProjectExecute);
            SaveAsProjectCommand = new RelayCommand(OnSaveAsProjectExecute);
            OpenProjectCommand = new RelayCommand(OnOpenProjectExecute);
            OpenRecentProjectCommand = new RelayCommand<object>(OnOpenRecentProjectExecute);

            OnLoadedCommand = new RelayCommand(OnLoadedExecute);
            _uniconProjectService.SetDialogContext(this);
        }

        private void OnNavigateToWebSyncViewExecute()
        {
	        
        }

        private async void OnOpenRecentProjectExecute(object project)
        {
	        if (project is RecentProjectViewModel recentProjectViewModel)
	        {
		       await _uniconProjectService.LoadProject(recentProjectViewModel.Path);
	        }
            OnProjectChanged();
        }

        private void OnLoadedExecute()
        {
	        _uniconProjectService.LoadDefaultProject();
	        OnProjectChanged();
        }

        private async void OnProjectChanged()
        {
	        await Task.Run(() =>
	        {
		        RaisePropertyChanged(nameof(ShellTitle));
		        RaisePropertyChanged(nameof(RecentProjects));
	        });
        }

        private void OnOpenOscillogramExecute()
        {
            _applicationGlobalCommands.OpenOscillogram();
        }

        public IFragmentPaneViewModel ActiveFragmentViewModel
        {
            get { return _activeFragmentViewModel; }
            set
            {
                _activeFragmentViewModel = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ActiveFragmentViewModel.FragmentTitle));
                ToolBarViewModel.SetDynamicOptionsGroup(ActiveFragmentViewModel?.FragmentViewModel
                    ?.FragmentOptionsViewModel);
            }
        }

        public ObservableCollection<IFragmentPaneViewModel> FragmentsOpenedCollection
        {
            get { return _fragmentsOpenedCollection; }
            set
            {
                _fragmentsOpenedCollection = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// свойство показывает открыто ли главное меню приложения
        /// </summary>
        public bool IsMenuFlyOutOpen
        {
            get => _isMenuFlyOutOpen;
            set
            {
                _isMenuFlyOutOpen = value;
                RaisePropertyChanged();
            }
        }

        public ILogService LogService { get; }

        public ObservableCollection<IAnchorableWindow> AnchorableWindows { get; set; }


        public RelayCommand<IDeviceViewModel> ChangeSelectedDevice { get; set; }
        public RelayCommand NavigateToDeviceEditorCommand { get; set; }
        public RelayCommand NavigateToWebSyncViewCommand { get; set; }

        public RelayCommand ExitCommand { get; set; }

        public ICommand NavigateToDeviceAddingCommand { get; set; }
        public ICommand OnClosingCommand { get; set; }
        public ICommand AddNewFragmentCommand { get; set; }

        private void OnNavigateToDeviceEditor()
        {
            IRegion runtimeRegion =
                _regionManager.Regions[ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME];
            if (runtimeRegion == null) return;
            Uri uri = new Uri(ApplicationGlobalNames.ViewNames.DEVICEEDITOR_VIEW_NAME, UriKind.Relative);

            _regionManager.RequestNavigate(ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME, uri,
                result =>
                {
                    if (result.Result == false)
                    {
                        throw new Exception(result.Error.Message);
                    }
                });

            IsMenuFlyOutOpen = false;
        }

        private void OnExit()
        {
            if (CheckExiting())
                _applicationGlobalCommands.ShutdownApplication();
        }

        private bool CheckExiting()
        {
            ProjectSaveCheckingResultEnum res = _uniconProjectService.CheckIfProjectSaved();
            if (res == ProjectSaveCheckingResultEnum.ProjectAlreadySaved)
            {
                if (_dialogCoordinator.ShowModalMessageExternal(this,
                        _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.EXIT),
                        _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.EXIT_QUESTION),
                        MessageDialogStyle.AffirmativeAndNegative) ==
                    MessageDialogResult.Affirmative)
                {
                    _applicationGlobalCommands.ShutdownApplication();
                }

                return false;
            }

            if ((res == ProjectSaveCheckingResultEnum.UserRejectedSaving) ||
                (res == ProjectSaveCheckingResultEnum.ProjectSavedByUserInDialog))
            {
                return true;
            }

            return false;
        }

        private void OnDeviceChanged(ConnectableItemChangingContext connectableItemChangingContext)
        {
	        switch (connectableItemChangingContext.ItemModifyingType)
	        {
		        case ItemModifyingTypeEnum.Edit:
			        IRegion runtimeRegion =
				        _regionManager.Regions[ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME];
			        if (runtimeRegion == null) return;
			        Uri uri = new Uri(ApplicationGlobalNames.ViewNames.DEVICEEDITING_VIEW_NAME, UriKind.Relative);
			        NavigationParameters parameters = new NavigationParameters();
			        parameters.Add(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY,
				        connectableItemChangingContext.Connectable);

			        _regionManager.RequestNavigate(
				        ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME,
				        uri,
				        result =>
				        {
					        if (result.Result == false)
					        {
						        throw new Exception(result.Error.Message);
					        }
				        }, parameters);

			        IsMenuFlyOutOpen = false;
			        break;
		        case ItemModifyingTypeEnum.Delete:

			        if (_applicationGlobalCommands.AskUserToDeleteSelectedGlobal(this))
			        {
				        IDeviceViewModel deviceViewModel = ProjectBrowserViewModel.DeviceViewModels.First((model =>
					        model.Model == connectableItemChangingContext.Connectable));
				        foreach (IFragmentViewModel fragment in deviceViewModel.FragmentViewModels)
				        {
					        IFragmentPaneViewModel openedFragment =
						        FragmentsOpenedCollection.FirstOrDefault((model =>
							        model.FragmentViewModel == fragment));
					        openedFragment?.FragmentPaneClosedAction?.Invoke(openedFragment);
				        }

				        ProjectBrowserViewModel.DeviceViewModels.Remove(deviceViewModel);
				        ActiveFragmentViewModel = null;
				        //закрываем соединение при удалении устройства
				        (connectableItemChangingContext.Connectable as IDevice).DeviceConnection.CloseConnection();
				        //надо удалить девайс из коллекции _devicesContainerService
				        _devicesContainerService.RemoveConnectableItem(
					        connectableItemChangingContext.Connectable as IDevice);
				        connectableItemChangingContext.Connectable.Dispose();
			        }

			        break;
		        case ItemModifyingTypeEnum.Add:
			        if (connectableItemChangingContext.Connectable != null)
				        ProjectBrowserViewModel.DeviceViewModels.Add(
					        _deviceViewModelFactory.CreateDeviceViewModel(
						        connectableItemChangingContext.Connectable as IDevice,
						        () => ActiveFragmentViewModel?.FragmentViewModel));
			        break;
		        case ItemModifyingTypeEnum.Refresh:
			        foreach (IDeviceViewModel deviceViewModel in ProjectBrowserViewModel.DeviceViewModels)
			        {
				        foreach (IFragmentViewModel fragment in deviceViewModel.FragmentViewModels)
				        {
					        IFragmentPaneViewModel openedFragment =
						        FragmentsOpenedCollection.FirstOrDefault(model =>
							        model.FragmentViewModel == fragment);
					        if (openedFragment != null)
					        {
						        FragmentsOpenedCollection.Remove(openedFragment);
					        }
				        }

				        ActiveFragmentViewModel = null;
				        (deviceViewModel.Model as IDisposable)?.Dispose();
			        }

			        ProjectBrowserViewModel.DeviceViewModels.Clear();
			        break;
		        case ItemModifyingTypeEnum.Connected:
		        {
			        IDeviceViewModel deviceViewModel = ProjectBrowserViewModel.DeviceViewModels.FirstOrDefault((model =>
				        model.Model == connectableItemChangingContext.Connectable));
			        if (deviceViewModel != null)
			        {
				        var index = ProjectBrowserViewModel.DeviceViewModels.IndexOf(deviceViewModel);

                        foreach (IFragmentViewModel fragment in deviceViewModel.FragmentViewModels)
				        {
					        IFragmentPaneViewModel openedFragment =
						        FragmentsOpenedCollection.FirstOrDefault((model =>
							        model.FragmentViewModel == fragment));
					        openedFragment?.FragmentPaneClosedAction?.Invoke(openedFragment);
				        }

				        ProjectBrowserViewModel.DeviceViewModels.Remove(deviceViewModel);
				        ActiveFragmentViewModel = null;
				        if (connectableItemChangingContext.Connectable != null)
					        ProjectBrowserViewModel.DeviceViewModels.Insert(index,
						        _deviceViewModelFactory.CreateDeviceViewModel(
							        connectableItemChangingContext.Connectable as IDevice,
							        () => ActiveFragmentViewModel?.FragmentViewModel));
			        }
		        }
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }

        private void OnExecuteAddNewFragment(IFragmentViewModel fragmentViewModel)
        {
            IFragmentPaneViewModel existingPane =
                FragmentsOpenedCollection.FirstOrDefault((model => model.FragmentViewModel == fragmentViewModel));
            if (existingPane != null)
            {
                ActiveFragmentViewModel = existingPane;
                return;
            }

            IFragmentPaneViewModel fragmentPaneViewModel =
                _fragmentPaneViewModelFactory.GetFragmentPaneViewModel(fragmentViewModel,
                    ProjectBrowserViewModel.DeviceViewModels);
            if (!FragmentsOpenedCollection.Contains(fragmentPaneViewModel))
            {
                FragmentsOpenedCollection.Add(fragmentPaneViewModel);
                fragmentPaneViewModel.FragmentPaneClosedAction += OnPaneClosed;
            }

            ActiveFragmentViewModel = fragmentPaneViewModel;
        }

        private void OnPaneClosed(IFragmentPaneViewModel fragmentPaneViewModel)
        {
            if (fragmentPaneViewModel == null) return;

            if (ActiveFragmentViewModel == fragmentPaneViewModel)
            {
                ActiveFragmentViewModel = null;
            }

            fragmentPaneViewModel.FragmentPaneClosedAction = null;
            FragmentsOpenedCollection.Remove(fragmentPaneViewModel);
            (fragmentPaneViewModel.FragmentViewModel as IDisposable)?.Dispose();
        }

        private void OnNavigateToDeviceAddingExecute()
        {
            IRegion runtimeRegion =
                _regionManager.Regions[ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME];
            if (runtimeRegion == null) return;
            Uri uri = new Uri(ApplicationGlobalNames.ViewNames.DEVICEEDITING_VIEW_NAME, UriKind.Relative);
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY, null);

            _regionManager.RequestNavigate(ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME, uri,
                result =>
                {
                    if (result.Result == false)
                    {
                        throw new Exception(result.Error.Message);
                    }
                }, parameters);


            IsMenuFlyOutOpen = false;
        }

        public ICommand NewProjectCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand OnLoadedCommand { get; }
        public ICommand SaveAsProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenOscillogramCommand { get; }
        public ICommand OpenRecentProjectCommand { get; }
        public ToolBarViewModel ToolBarViewModel { get; }

        public List<RecentProjectViewModel> RecentProjects => _recentProjectsViewModelFactory.CreateProjectViewModels();

        public string ShellTitle => _uniconProjectService.GetProjectTitle();

        private void OnSaveAsProjectExecute()
        {
            _uniconProjectService.SaveProjectAs();
            OnProjectChanged();
        }

        private void OnSaveProjectExecute()
        {
            _uniconProjectService.SaveProject();
            OnProjectChanged();
        }

        private void OnNewProjectExecute()
        {
            _uniconProjectService.CreateNewProject();
            OnProjectChanged();
        }

        private void OnOpenProjectExecute()
        {

	        _uniconProjectService.OpenProject();
	        OnProjectChanged();

        }

        private void OnExecuteClosing(CancelEventArgs cancelEventArgs)
        {
            if (CheckExiting()) return;

            if (cancelEventArgs != null) cancelEventArgs.Cancel = true;
        }
    }
}