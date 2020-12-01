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
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Presentation.Subscription;
using Unicon2.Presentation.ViewModels;
using Unicon2.Shell.Factories;
using Unicon2.Shell.ViewModels.Helpers;
using Unicon2.Shell.ViewModels.MenuItems;
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
        private readonly IMainMenuService _mainMenuService;
        private ToggleOptionsMenuItemViewModel _toggleOptionsMenuItemViewModel;
        private RecentProjectsMenuItemViewModel _recentProjectsMenuItemViewModel;
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
            RecentProjectsViewModelFactory recentProjectsViewModelFactory,
            IMainMenuService mainMenuService,
            DynamicMainMenuViewModel dynamicMainMenuViewModel,
            IFlyoutService flyoutService
            ,IGlobalEventManager globalEventManger)
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
            DynamicMainMenuViewModel = dynamicMainMenuViewModel;
            _recentProjectsViewModelFactory = recentProjectsViewModelFactory;
            _mainMenuService = mainMenuService;
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
            _toggleOptionsMenuItemViewModel = new ToggleOptionsMenuItemViewModel(this);
            _mainMenuService.RegisterMainMenuItem(new MainMenuRegistrationOptions(Guid.NewGuid(),
                _toggleOptionsMenuItemViewModel
            ));

            _mainMenuService.RegisterMainMenuItemGroup(new MainMenuGroupRegistrationOptions(Guid.NewGuid(),
                ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                OpenProjectCommand, ApplicationGlobalNames.UiCommandStrings.OPEN_PROJECT,
                100, ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                NewProjectCommand, ApplicationGlobalNames.UiCommandStrings.NEW_PROJECT,
                100, ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY
            ));

            _recentProjectsMenuItemViewModel = new RecentProjectsMenuItemViewModel(OpenRecentProjectCommand);

            _mainMenuService.RegisterMainMenuItem(new MainMenuRegistrationOptions(Guid.NewGuid(),
                _recentProjectsMenuItemViewModel, 100, ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                SaveProjectCommand, ApplicationGlobalNames.UiCommandStrings.SAVE_PROJECT,
                100, ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                SaveAsProjectCommand, ApplicationGlobalNames.UiCommandStrings.SAVE_AS_PROJECT,
                100, ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                ExitCommand, ApplicationGlobalNames.UiCommandStrings.EXIT,
                100, ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuItemGroup(new MainMenuGroupRegistrationOptions(Guid.NewGuid(),
                ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                NavigateToDeviceAddingCommand, ApplicationGlobalNames.UiCommandStrings.ADD,
                100, ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                NavigateToDeviceEditorCommand, ApplicationGlobalNames.UiCommandStrings.OPEN_DEVICE_EDITOR,
                100, ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY
            ));

            _mainMenuService.RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(Guid.NewGuid(),
                OpenOscillogramCommand, ApplicationGlobalNames.UiCommandStrings.OPEN_OSC));

            _uniconProjectService.LoadDefaultProject();

            OnProjectChanged();
            _applicationGlobalCommands.ShellLoaded?.Invoke();
        }

        private async void OnProjectChanged()
        {
	        await Task.Run(() =>
	        {
		        RaisePropertyChanged(nameof(ShellTitle));
		        RaisePropertyChanged(nameof(RecentProjects));
		        if (_recentProjectsMenuItemViewModel != null)
			        _recentProjectsMenuItemViewModel.RecentProjects = RecentProjects;
	        });
        }

        private void OnOpenOscillogramExecute()
        {
            _applicationGlobalCommands.OpenOscillogram();
        }

        public IFragmentPaneViewModel ActiveFragmentViewModel
        {
            get => _activeFragmentViewModel;
            set
            {
                TrySetFragmentOpened(_activeFragmentViewModel,false);
                _activeFragmentViewModel = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ActiveFragmentViewModel.FragmentTitle));
                ToolBarViewModel.SetDynamicOptionsGroup(ActiveFragmentViewModel?.FragmentViewModel
                    ?.FragmentOptionsViewModel);
            }
        }

        private async void TrySetFragmentOpened(IFragmentPaneViewModel fragmentPaneViewModel, bool isOpened)
        {
            if (fragmentPaneViewModel?.FragmentViewModel is IFragmentOpenedListener fragmentOpenedListener)
            {
                await fragmentOpenedListener.SetFragmentOpened(isOpened);
            }
        }

        public ObservableCollection<IFragmentPaneViewModel> FragmentsOpenedCollection
        {
            get => _fragmentsOpenedCollection;
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
                _toggleOptionsMenuItemViewModel?.Update();
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

        private async void OnDeviceChanged(ConnectableItemChangingContext connectableItemChangingContext)
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
                    {
                        var devicevm = _deviceViewModelFactory.CreateDeviceViewModel(this,
                            connectableItemChangingContext.Connectable as IDevice);
                        ProjectBrowserViewModel.DeviceViewModels.Add(devicevm);

                        if (connectableItemChangingContext.Connectable.DeviceConnection is IDataProvider dataProvider)
                        {
                            dataProvider.TransactionCompleteSubscription =
                                devicevm.TransactionCompleteSubscription;
                            (devicevm.TransactionCompleteSubscription as TransactionCompleteSubscription)
                                ?.ResetOnConnectionRetryCounter(true);

                        }

                        foreach (IFragmentViewModel fragment in devicevm.FragmentViewModels)
                        {
                            if (fragment is IFragmentConnectionChangedListener fragmentConnectionChangedListener)
                            {
                                await fragmentConnectionChangedListener.OnConnectionChanged();
                            }
                        }
                    }

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
                        if (connectableItemChangingContext.Connectable.DeviceConnection is IDataProvider dataProvider)
                        {
                            dataProvider.TransactionCompleteSubscription =
                                deviceViewModel.TransactionCompleteSubscription;
                            (deviceViewModel.TransactionCompleteSubscription as TransactionCompleteSubscription)?.ResetOnConnectionRetryCounter(true);
                        }
                        deviceViewModel.TransactionCompleteSubscription.Execute();
                        foreach (IFragmentViewModel fragment in deviceViewModel.FragmentViewModels)
				        {
                            if (fragment is IFragmentConnectionChangedListener fragmentConnectionChangedListener)
                            {
                                await fragmentConnectionChangedListener.OnConnectionChanged();
                            }
				        }
                    }
		        }
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }

        private async void OnExecuteAddNewFragment(IFragmentViewModel fragmentViewModel)
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
            
            if (fragmentViewModel is IFragmentOpenedListener fragmentOpenedListener)
            {
                await fragmentOpenedListener.SetFragmentOpened(true);
            }
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
        public DynamicMainMenuViewModel DynamicMainMenuViewModel { get; }

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