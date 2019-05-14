using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Factories
{
    public interface IBrowserElementFactory
    {
        IDeviceDirectory CreateRootDeviceDirectoryBrowserElement();
        IDeviceDirectory CreateDeviceDirectoryBrowserElement(string path,IDeviceDirectory parentDeviceDirectory);
        IDeviceFile CreateDeviceFileBrowserElement(string path, IDeviceDirectory parentDeviceDirectory);

        IDeviceBrowserElement CreateBrowserElement( string path, IDeviceDirectory parentDeviceDirectory);
        void SetConnectionProvider(object dataProvider);
    }
}