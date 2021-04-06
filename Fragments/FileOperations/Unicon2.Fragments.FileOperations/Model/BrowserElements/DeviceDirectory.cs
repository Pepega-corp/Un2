using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.Model.BrowserElements
{
    public class DeviceDirectory : BrowserElementBase, IDeviceDirectory
    {
        private IDirectoryLoader _directoryLoader;

        public DeviceDirectory(string directoryPath, IDirectoryLoader directoryLoader, string name, IDeviceDirectory deviceDirectory)
            : base(directoryPath, name, deviceDirectory)
        {
            this._directoryLoader = directoryLoader;
        }


        public override async Task Load()
        {
            this.BrowserElementsInDirectory = await this._directoryLoader.LoadDeviceDirectory(ElementPath, this);
            foreach (IDeviceBrowserElement browserElement in this.BrowserElementsInDirectory)
            {
                if (browserElement is ILoadable)
                {
                    await browserElement.Load();
                }
            }
        }

        public override string StrongName => FileOperationsKeys.DEVICE_DIRECTORY;

        public List<IDeviceBrowserElement> BrowserElementsInDirectory { get; private set; }

        public async Task<bool> RemoveChildElementAsync(IDeviceBrowserElement browserElement)
        {
            return await this._directoryLoader.RemoveElementFromDirectory(browserElement);
        }

        public async Task<bool> CreateNewChildDirectoryAsync(string directoryName)
        {
            return await this._directoryLoader.CreateNewChildDirectoryAsync(ElementPath + "\\" + directoryName);
        }

        public async Task AddNewChildFileAsync(byte[] file, string name, string extension)
        {
            await this._directoryLoader.CreateNewChildFileAsync(file, name, extension);
        }


        public override DeviceContext DeviceContext
        {
            get => base.DeviceContext;
            set
            {
                this.BrowserElementsInDirectory?.ForEach(element => element.DataProvider = value.DataProviderContainer.DataProvider);
                this._directoryLoader.SetDeviceContext(value);
                base.DeviceContext = value;
            }
        }
    }
}
