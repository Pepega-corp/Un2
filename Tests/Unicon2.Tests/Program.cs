using Prism.Ioc;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Model.DefaultDevice;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Shell;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils.Mocks;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Tests
{
    public static class Program
    {

        private static App _app;
        private static DefaultDevice _defaultDevice;

        static void Main(string[] args)
        {
        }

        public static App GetApp()
        {
            if (_app == null)
            {
                _app = new App();
                _app.InitializePublic();
                var shell = _app.Container.Resolve<ShellViewModel>();
            }
            return _app;
        }


        public static void RefreshProject()
        {
            GetApp().Container.Resolve<IDevicesContainerService>().Refresh();
            GetApp().Container.Resolve<IDevicesContainerService>().ConnectableItemChanged?.Invoke(
                new ConnectableItemChangingContext(null, ItemModifyingTypeEnum.Refresh));
            GetApp().Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(GetDevice());
        }

        public static DefaultDevice GetDevice()
        {
            if (_defaultDevice == null)
            {     

                var serializerService = GetApp().Container.Resolve<ISerializerService>();

                _defaultDevice = serializerService.DeserializeFromFile<IDevice>("FileAssets/testFile.json") as DefaultDevice;
                GetApp().Container.Resolve<IDevicesContainerService>()
                    .AddConnectableItem(_defaultDevice);
            }
            return _defaultDevice;
        }
        
    }
}