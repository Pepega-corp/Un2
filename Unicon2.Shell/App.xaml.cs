using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Unicon2.Connections.MockConnection.Module;
using Unicon2.Connections.ModBusRtuConnection.Module;
using Unicon2.Connections.ModBusTcpConnection.Module;
using Unicon2.Connections.OfflineConnection;
using Unicon2.Formatting.Editor.Module;
using Unicon2.Formatting.Module;
using Unicon2.Fragments.Configuration.Editor.Module;
using Unicon2.Fragments.Configuration.Matrix.Module;
using Unicon2.Fragments.Configuration.Module;
using Unicon2.Fragments.Journals.Editor.Module;
using Unicon2.Fragments.Journals.Module;
using Unicon2.Fragments.Measuring.Editor.Module;
using Unicon2.Fragments.Measuring.Module;
//using Unicon2.Fragments.DateTime.Module;
//using Unicon2.Fragments.DateTime.Editor.Module;
//using Unicon2.Fragments.Configuration.Exporter.Module;
//using Unicon2.Fragments.FileOperations.Editor.Module;
//using Unicon2.Fragments.FileOperations.Module;
//using Unicon2.Fragments.Journals.Editor.Module;
//using Unicon2.Fragments.Journals.Module;
//using Unicon2.Fragments.Measuring.Editor.Module;
//using Unicon2.Fragments.Measuring.Module;
using Unicon2.Fragments.ModbusMemory.Module;
//using Unicon2.Fragments.Oscilliscope.Editor.Module;
//using Unicon2.Fragments.Oscilliscope.Module;
//using Unicon2.Fragments.Programming.Editor.Module;
//using Unicon2.Fragments.Programming.Module;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Model.Module;
using Unicon2.ModuleDeviceEditing;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Module;
using Unicon2.Presentation.ViewModels;
using Unicon2.Services.Module;
using Unicon2.Shell.ControlRegionAdapter;
using Unicon2.Shell.Services;
using Unicon2.Shell.ViewModels;
using Unicon2.Shell.Views;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private UniconSplashScreen _uniconSplashScreen;
        private AutoResetEvent WaitForCreation { get; set; }
        private bool _bootstrapperInitialized;
        public Action<string> BootsrapperMessageAction;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.Length > 1)
            {
                MessageBox.Show("Приложение \"Unicon\" уже запущено", "Внимание", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                Environment.Exit(0);
                //Current.Shutdown();
            }
            else
            {
                _bootstrapperInitialized = false;
                UniconSplashScreenViewModel uniconSplashScreenViewModel = new UniconSplashScreenViewModel(this);
                WaitForCreation = new AutoResetEvent(false);
                //local function
                void ShowSplash()
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() =>
                   {
                       _uniconSplashScreen = new UniconSplashScreen();
                       _uniconSplashScreen.DataContext = uniconSplashScreenViewModel;
                       _uniconSplashScreen.Show();
                       WaitForCreation.Set();
                   }));

                    Dispatcher.Run();
                }

                Thread thread = new Thread(ShowSplash) { Name = "Splash Thread", IsBackground = true };
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                WaitForCreation.WaitOne();
                _uniconSplashScreen.Closed += (o, ea) =>
                {
                    if (!_bootstrapperInitialized)
                    {
                        Environment.Exit(0);
                    }
                };
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            string message = $"Возникло необрабатываемое исключение: {e.Exception.Message}\n {e.Exception.StackTrace}";
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            BootsrapperMessageAction?.Invoke("Register Types");
            //Register TypesContainer that represent IUnityContainer
            containerRegistry.RegisterSingleton<ITypesContainer, TypesContainer>();
            StaticContainer.SetContainer(Container.Resolve<ITypesContainer>());
            containerRegistry.RegisterInstance(DialogCoordinator.Instance);
            containerRegistry.Register<IDeviceDefinitionViewModel, DeviceDefinitionViewModel>();
            containerRegistry.RegisterInstance<IApplicationSettingsService>(new ApplicationSettingsService());
            containerRegistry.RegisterInstance(new ShellSettingsViewModel(StaticContainer.Container));
            //регистрация вью-моделей
            containerRegistry.Register<Views.Shell>();
            StaticContainer.Container.RegisterViewModel<Views.Shell, ShellViewModel>();
            StaticContainer.Container.RegisterViewModel<ShellSettingsFlyOut, ShellSettingsViewModel>();
            //модули
            RegisterModuleCatalogs(StaticContainer.Container);
            InitializeUnityModules();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Views.Shell>();
        }

        protected override void InitializeShell(Window shell)
        {
            Current.MainWindow = shell;
            Current.MainWindow?.Show();
            Current.MainWindow?.Activate();
            _bootstrapperInitialized = true;
            _uniconSplashScreen.Dispatcher.Invoke(_uniconSplashScreen.Close);
            ShellSettingsViewModel settingsViewModel = Container.Resolve<ShellSettingsViewModel>();
            settingsViewModel.Initialize();
        }

        protected void RegisterModuleCatalogs(ITypesContainer container)
        {
            BootsrapperMessageAction?.Invoke("Adding Modules To Catalog");
            container.Register<IUnityModule, ServicesModule>(nameof(ServicesModule));
            container.Register<IUnityModule, ModuleDeviceEditingModule>(nameof(ModuleDeviceEditingModule));
            container.Register<IUnityModule, MockConnectionModule>(nameof(MockConnectionModule));
            container.Register<IUnityModule, ModbusRtuConnectionModule>(nameof(ModbusRtuConnectionModule));
            container.Register<IUnityModule, ModBusTcpModule>(nameof(ModBusTcpModule));
            container.Register<IUnityModule, OfflineConnectionModule>(nameof(OfflineConnectionModule));
            container.Register<IUnityModule, MockConnectionModule>(nameof(MockConnectionModule));
            container.Register<IUnityModule, DeviceEditorUtilityModule.Module.DeviceEditorUtilityModule>(nameof(DeviceEditorUtilityModule.Module.DeviceEditorUtilityModule));
            container.Register<IUnityModule, UniconModelModule>(nameof(UniconModelModule));
            container.Register<IUnityModule, PresentationModule>(nameof(PresentationModule));
            container.Register<IUnityModule, FormattingModule>(nameof(FormattingModule));
            container.Register<IUnityModule, FormattingEditorModule>(nameof(FormattingEditorModule));
            container.Register<IUnityModule, ConfigurationModule>(nameof(ConfigurationModule));
           // container.Register<IUnityModule, ConfigurationExporterModule>(nameof(ConfigurationExporterModule));
            container.Register<IUnityModule, ConfigurationEditorModule>(nameof(ConfigurationEditorModule));
            //container.Register<IUnityModule, UniconDateTimeModule>(nameof(UniconDateTimeModule));
            //container.Register<IUnityModule, UniconDateTimeEditorModule>(nameof(UniconDateTimeEditorModule)); 
            //container.Register<IUnityModule, MatrixConfigurationModule>(nameof(MatrixConfigurationModule));
            container.Register<IUnityModule, ModbusMemoryModule>(nameof(ModbusMemoryModule));
            container.Register<IUnityModule, JournaEditorModule>(nameof(JournaEditorModule));
            container.Register<IUnityModule, UniconJournalModule>(nameof(UniconJournalModule));
			container.Register<IUnityModule, MeasuringModule>(nameof(MeasuringModule));
			container.Register<IUnityModule, MeasuringEditorModule>(nameof(MeasuringEditorModule));
			//container.Register<IUnityModule, OscilloscopeModule>(nameof(OscilloscopeModule));
			//container.Register<IUnityModule, OscilloscopeEditorModule>(nameof(OscilloscopeEditorModule));
			//container.Register<IUnityModule, FileOperationsModule>(nameof(FileOperationsModule));
			//container.Register<IUnityModule, FileOperationsEditorModule>(nameof(FileOperationsEditorModule));

			//container.Register<IUnityModule, ProgrammingModule>(nameof(ProgrammingModule));
			//container.Register<IUnityModule, ProgrammingEditorModule>(nameof(ProgrammingEditorModule));
		}

        protected override void InitializeModules()
        {
            base.InitializeModules();

            BootsrapperMessageAction?.Invoke("Initializing Devices");
            IDevicesContainerService devicesContainerService = StaticContainer.Container.Resolve<IDevicesContainerService>();
            devicesContainerService.LoadDevicesDefinitions();
        }

        private void InitializeUnityModules()
        {
            BootsrapperMessageAction?.Invoke("Initializing Modules");
            System.Collections.Generic.IEnumerable<IUnityModule> modules = StaticContainer.Container.ResolveAll<IUnityModule>();
            foreach (IUnityModule module in modules)
            {
                module.Initialize(StaticContainer.Container);
            }
        }


        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping(typeof(FlyoutsControl), StaticContainer.Container.Resolve<FlyoutsControlRegionAdapter>());
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }
    }
}
