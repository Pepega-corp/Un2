using MahApps.Metro.Controls.Dialogs;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Services.GeneralFactories;
using Unicon2.Services.LogService;
using Unicon2.Services.UniconProject;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Services.Module
{
    public class ServicesModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.RegisterInstance<ILocalizerService>(new LocalizerService("ru-RU"));
            container.Register<ILogMessage, LogMessage>();
            container.Register<IDeviceLogger, DeviceLogger>();
            container.Register<IDevicesContainerService, DevicesContainerService>(true);
            container.Register<IXamlResourcesService, ApplicationXamlResourcesService>(true);

            container.Register<ISerializerService, SerializerService.SerializerService>(true);

            container.Register<IUniconProject, UniconProject.UniconProject>();
            container.Register<IUniconProjectService, UniconProjectService>(true);

            container.Register(typeof(IGeneralViewModelFactory<>), typeof(GeneralViewModelFactory<>));
            container.Register<IExceptionLoggerService, ExceptionLoggerService>();

            container.Register<ILogService, LogService.LogService>(true);

            container.RegisterInstance<IApplicationGlobalCommands>(
                new ApplicationGlobalCommands(container, container.Resolve<IDialogCoordinator>(), container.Resolve<ILocalizerService>()));

           // container.Resolve<ISerializerService>().AddKnownTypeForSerialization(typeof(DeviceLogger));
           // container.Resolve<ISerializerService>().AddNamespaceAttribute("deviceLogger", "DeviceLoggerNS");
        }
    }
}