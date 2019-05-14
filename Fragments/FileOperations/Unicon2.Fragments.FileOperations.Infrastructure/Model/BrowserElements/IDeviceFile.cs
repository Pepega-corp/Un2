using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements
{
    public interface IDeviceFile: IDeviceBrowserElement,ILoadable
    {
        byte[] FileData { get; }
        void Download();
    }
}