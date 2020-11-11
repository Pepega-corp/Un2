using Prism.Ioc;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Shell;
using Unicon2.Shell.ViewModels;

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
        public static DefaultDevice GetDevice()
        {
            if (_defaultDevice == null)
            {
                var serializerService = GetApp().Container.Resolve<ISerializerService>();

                _defaultDevice = serializerService.DeserializeFromFile<IDevice>("testFile.json") as DefaultDevice;
                GetApp().Container.Resolve<IDevicesContainerService>()
                    .AddConnectableItem(_defaultDevice);
            }
            return _defaultDevice;
        }
        
    }
}