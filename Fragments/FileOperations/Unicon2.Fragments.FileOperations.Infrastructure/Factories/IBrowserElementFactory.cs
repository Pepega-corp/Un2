using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Factories
{
    public interface IBrowserElementFactory : IDeviceContextConsumer
    {
        IDeviceDirectory CreateRootDeviceDirectoryBrowserElement();
        IDeviceDirectory CreateDeviceDirectoryBrowserElement(string path,IDeviceDirectory parentDeviceDirectory);
        IDeviceFile CreateDeviceFileBrowserElement(string path, IDeviceDirectory parentDeviceDirectory);
        IDeviceBrowserElement CreateBrowserElement( string path, IDeviceDirectory parentDeviceDirectory);
    }
}