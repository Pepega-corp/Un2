using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders
{
    public interface IDirectoryLoader
    {
        Task<List<IDeviceBrowserElement>> LoadDeviceDirectory(string directoryPath,IDeviceDirectory parentDeviceDirectory);
        Task<bool> RemoveElementFromDirectory(IDeviceBrowserElement deviceBrowserElement);
        Task<bool> CreateNewChildDirectoryAsync(string directoryPath);
        Task<string> CreateNewChildFileAsync(byte[] fileBytes,string directoryPath,string fileName,string extension);
        void SetDeviceContext(DeviceContext deviceContext);
    }
}