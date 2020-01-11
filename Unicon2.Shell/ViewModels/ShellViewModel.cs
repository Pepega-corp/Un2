using MahApps.Metro.Controls.Dialogs;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            IUniconProjectService uniconProjectService,ToolBarViewModel toolBarViewModel)
        {
            this.LogServiceViewModel = logServiceViewModel;
            this.ProjectBrowserViewModel = projectBrowserViewModel;
            this.LogService = logService;
            this._devicesContainerService = devicesContainerService;
            this._devicesContainerService.ConnectableItemChanged += this.OnDeviceChanged;
            this._dialogCoordinator = dialogCoordinator;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this._regionManager = regionManager;
            this._localizerService = localizerService;
            this._deviceViewModelFactory = deviceViewModelFactory;
            this._fragmentPaneViewModelFactory = fragmentPaneViewModelFactory;
            this._uniconProjectService = uniconProjectService;

            this.ExitCommand = new RelayCommand(this.OnExit);
            this.NavigateToDeviceEditorCommand = new RelayCommand(this.OnNavigateToDeviceEditor);
            this.NavigateToDeviceAddingCommand = new RelayCommand(this.OnNavigateToDeviceAddingExecute);
            this.AddNewFragmentCommand = new RelayCommand<IFragmentViewModel>(this.OnExecuteAddNewFragment);
            this._fragmentsOpenedCollection = new ObservableCollection<IFragmentPaneViewModel>();
            this.OpenOscillogramCommand = new RelayCommand(this.OnOpenOscillogramExecute);
            this.OnClosingCommand = new RelayCommand<CancelEventArgs>(this.OnExecuteClosing);
            this.AnchorableWindows = new ObservableCollection<IAnchorableWindow>
            {
                this.ProjectBrowserViewModel, this.LogServiceViewModel
            };
            ToolBarViewModel = toolBarViewModel;
            StaticOptionsButtonsHelper.InitializeStaticButtons(this);
            this.NewProjectCommand = new RelayCommand(this.OnNewProjectExecute);
            this.SaveProjectCommand = new RelayCommand(this.OnSaveProjectExecute);
            this.SaveAsProjectCommand = new RelayCommand(this.OnSaveAsProjectExecute);
            this.OpenProjectCommand = new RelayCommand(this.OnOpenProjectExecute);
        }

       

        private void OnOpenOscillogramExecute()
        {
            this._applicationGlobalCommands.OpenOscillogram();
        }

        public IFragmentPaneViewModel ActiveFragmentViewModel
        {
            get { return this._activeFragmentViewModel; }
            set
            {
                this._activeFragmentViewModel = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.ActiveFragmentViewModel.FragmentTitle));
                ToolBarViewModel.SetDynamicOptionsGroup(ActiveFragmentViewModel?.FragmentViewModel?.FragmentOptionsViewModel);
            }
        }

        public ObservableCollection<IFragmentPaneViewModel> FragmentsOpenedCollection
        {
            get { return this._fragmentsOpenedCollection; }
            set
            {
                this._fragmentsOpenedCollection = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// свойство показывает открыто ли главное меню приложения
        /// </summary>
        public bool IsMenuFlyOutOpen
        {
            get => this._isMenuFlyOutOpen;
            set
            {
                this._isMenuFlyOutOpen = value;
                this.RaisePropertyChanged();
            }
        }

        public ILogService LogService { get; }

        public ObservableCollection<IAnchorableWindow> AnchorableWindows { get; set; }


        public RelayCommand<IDeviceViewModel> ChangeSelectedDevice { get; set; }
        public RelayCommand NavigateToDeviceEditorCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        public ICommand NavigateToDeviceAddingCommand { get; set; }
        public ICommand OnClosingCommand { get; set; }
        public ICommand AddNewFragmentCommand { get; set; }

        private void OnNavigateToDeviceEditor()
        {
            IRegion runtimeRegion = this._regionManager.Regions[ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME];
            if (runtimeRegion == null) return;
            Uri uri = new Uri(ApplicationGlobalNames.ViewNames.DEVICEEDITOR_VIEW_NAME, UriKind.Relative);

            this._regionManager.RequestNavigate(ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME, uri,
                result =>
                {
                    if (result.Result == false)
                    {
                        throw new Exception(result.Error.Message);
                    }
                });

            this.IsMenuFlyOutOpen = false;
        }

        private void OnExit()
        {
            if (this.CheckExiting())
                this._applicationGlobalCommands.ShutdownApplication();
        }

        private bool CheckExiting()
        {
            ProjectSaveCheckingResultEnum res = this._uniconProjectService.CheckIfProjectSaved(this);
            if (res == ProjectSaveCheckingResultEnum.ProjectAlreadySaved)
            {
                if (this._dialogCoordinator.ShowModalMessageExternal(this, this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.EXIT), this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.EXIT_QUESTION),
                        MessageDialogStyle.AffirmativeAndNegative) ==
                    MessageDialogResult.Affirmative)
                {
                    this._applicationGlobalCommands.ShutdownApplication();
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
                    IRegion runtimeRegion = this._regionManager.Regions[ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME];
                    if (runtimeRegion == null) return;
                    Uri uri = new Uri(ApplicationGlobalNames.ViewNames.DEVICEEDITING_VIEW_NAME, UriKind.Relative);
                    NavigationParameters parameters = new NavigationParameters();
                    parameters.Add(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY, connectableItemChangingContext.Connectable);

                    this._regionManager.RequestNavigate(ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME,
                        uri,
                        result =>
                        {
                            if (result.Result == false)
                            {
                                throw new Exception(result.Error.Message);
                            }
                        }, parameters);

                    this.IsMenuFlyOutOpen = false;
                    break;
                case ItemModifyingTypeEnum.Delete:

                    if (this._applicationGlobalCommands.AskUserToDeleteSelectedGlobal(this))
                    {
                        IDeviceViewModel deviceViewModel = this.ProjectBrowserViewModel.DeviceViewModels.First((model => model.Model == connectableItemChangingContext.Connectable));
                        foreach (IFragmentViewModel fragment in deviceViewModel.FragmentViewModels)
                        {
                            IFragmentPaneViewModel openedFragment = this.FragmentsOpenedCollection.FirstOrDefault((model => model.FragmentViewModel == fragment));
                            openedFragment?.FragmentPaneClosedAction?.Invoke(openedFragment);
                        }
                        this.ProjectBrowserViewModel.DeviceViewModels.Remove(deviceViewModel);
                        this.ActiveFragmentViewModel = null;
                        //закрываем соединение при удалении устройства
                        (connectableItemChangingContext.Connectable as IDevice).DeviceConnection.CloseConnection();
                        //надо удалить девайс из коллекции _devicesContainerService
                        _devicesContainerService.RemoveConnectableItem(connectableItemChangingContext.Connectable as IDevice);
                        connectableItemChangingContext.Connectable.Dispose();
                    }

                    break;
                case ItemModifyingTypeEnum.Add:
                    if (connectableItemChangingContext.Connectable != null)
                        this.ProjectBrowserViewModel.DeviceViewModels.Add(this._deviceViewModelFactory.CreateDeviceViewModel(connectableItemChangingContext.Connectable as IDevice));
                    break;
                case ItemModifyingTypeEnum.Refresh:
                    foreach (IDeviceViewModel deviceViewModel in this.ProjectBrowserViewModel.DeviceViewModels)
                    {
                        foreach (IFragmentViewModel fragment in deviceViewModel.FragmentViewModels)
                        {
                            IFragmentPaneViewModel openedFragment = this.FragmentsOpenedCollection.FirstOrDefault(model => model.FragmentViewModel == fragment);
                            if (openedFragment != null)
                            {
                                this.FragmentsOpenedCollection.Remove(openedFragment);
                            }
                        }

                        this.ActiveFragmentViewModel = null;
                        (deviceViewModel.Model as IDisposable)?.Dispose();
                    }
                    this.ProjectBrowserViewModel.DeviceViewModels.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnExecuteAddNewFragment(IFragmentViewModel fragmentViewModel)
        {
            IFragmentPaneViewModel existingPane = this.FragmentsOpenedCollection.FirstOrDefault((model => model.FragmentViewModel == fragmentViewModel));
            if (existingPane != null)
            {
                this.ActiveFragmentViewModel = existingPane;
                return;
            }
            IFragmentPaneViewModel fragmentPaneViewModel = this._fragmentPaneViewModelFactory.GetFragmentPaneViewModel(fragmentViewModel, this.ProjectBrowserViewModel.DeviceViewModels);
            if (!this.FragmentsOpenedCollection.Contains(fragmentPaneViewModel))
            {
                this.FragmentsOpenedCollection.Add(fragmentPaneViewModel);
                fragmentPaneViewModel.FragmentPaneClosedAction += this.OnPaneClosed;
            }
            this.ActiveFragmentViewModel = fragmentPaneViewModel;
        }

        private void OnPaneClosed(IFragmentPaneViewModel fragmentPaneViewModel)
        {
            if (fragmentPaneViewModel == null) return;

            if (this.ActiveFragmentViewModel == fragmentPaneViewModel)
            {
                this.ActiveFragmentViewModel = null;
            }

            fragmentPaneViewModel.FragmentPaneClosedAction = null;
            this.FragmentsOpenedCollection.Remove(fragmentPaneViewModel);
            (fragmentPaneViewModel.FragmentViewModel as IDisposable)?.Dispose();
        }

        private void OnNavigateToDeviceAddingExecute()
        {
            IRegion runtimeRegion = this._regionManager.Regions[ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME];
            if (runtimeRegion == null) return;
            Uri uri = new Uri(ApplicationGlobalNames.ViewNames.DEVICEEDITING_VIEW_NAME, UriKind.Relative);
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY, null);

            this._regionManager.RequestNavigate(ApplicationGlobalNames.ViewNames.DEVICE_EDITING_FLYOUT_REGION_NAME, uri,
                result =>
                {
                    if (result.Result == false)
                    {
                        throw new Exception(result.Error.Message);
                    }
                }, parameters);


            this.IsMenuFlyOutOpen = false;
        }

        public ICommand NewProjectCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand SaveAsProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand OpenOscillogramCommand { get; }
        public ToolBarViewModel ToolBarViewModel { get; }
        private void OnSaveAsProjectExecute()
        {
            this._uniconProjectService.SaveProjectAs();
        }

        private void OnSaveProjectExecute()
        {
            this._uniconProjectService.SaveProject();
        }

        private void OnNewProjectExecute()
        {
            this._uniconProjectService.CreateNewProject();
        }

        private void OnOpenProjectExecute()
        {
            this._uniconProjectService.OpenProject("",this);
        }

        private void OnExecuteClosing(CancelEventArgs cancelEventArgs)
        {
            if (this.CheckExiting()) return;

            if (cancelEventArgs != null) cancelEventArgs.Cancel = true;
        }
    }
}