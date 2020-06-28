using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.Model.Loaders
{
    public class DirectoryLoader : IDirectoryLoader
    {
        private readonly IBrowserElementFactory _browserElementFactory;
        private readonly IFileDriver _fileDriver;

        public DirectoryLoader(IBrowserElementFactory browserElementFactory, IFileDriver fileDriver)
        {
            this._browserElementFactory = browserElementFactory;
            this._fileDriver = fileDriver;
        }

        public async Task<List<IDeviceBrowserElement>> LoadDeviceDirectory(string directoryPath, IDeviceDirectory parentDeviceDirectory)
        {
            List<IDeviceBrowserElement> deviceBrowserElements = new List<IDeviceBrowserElement>();
            List<string> directoryList = await this._fileDriver.GetDirectoryByPath(directoryPath);
            directoryList.ForEach((path =>
                deviceBrowserElements.Add(this._browserElementFactory.CreateBrowserElement(path, parentDeviceDirectory))));
            foreach (IDeviceBrowserElement element in deviceBrowserElements)
            {
                if (element is IDeviceDirectory)
                {
                    await (element as IDeviceDirectory).Load();
                }
            }
            return deviceBrowserElements;
        }

        public async Task<bool> RemoveElementFromDirectory(IDeviceBrowserElement deviceBrowserElement)
        {
            return await this._fileDriver.DeleteElement(deviceBrowserElement.Name);
        }

        public async Task<bool> CreateNewChildDirectoryAsync(string directoryPath)
        {
            return await this._fileDriver.CreateDirectory(directoryPath);
        }

        public async Task<string> CreateNewChildFileAsync(byte[] fileBytes, string directoryPath, string fileName, string extension)
        {
            return await this._fileDriver.WriteFile(fileBytes, directoryPath, $"{fileName}.{extension}");
        }

        public void SetDeviceContext(DeviceContext deviceContext) => _fileDriver.DeviceContext = deviceContext;
    }
}
