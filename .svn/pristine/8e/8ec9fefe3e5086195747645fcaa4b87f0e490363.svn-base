using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.FileOperations.Model.BrowserElements
{
    public class DeviceDirectory : BrowserElementBase, IDeviceDirectory
    {
        private IDirectoryLoader _directoryLoader;
        private string _strongName;

        public DeviceDirectory(string directoryPath, IDirectoryLoader directoryLoader, string name, IDeviceDirectory deviceDirectory)
            : base(directoryPath, name, deviceDirectory)
        {
            this._directoryLoader = directoryLoader;
        }



        #region Implementation of IDataProviderContaining



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

        #endregion

        #region Implementation of IDeviceDirectory

        public List<IDeviceBrowserElement> BrowserElementsInDirectory { get; private set; }

        public async Task<bool> RemoveChildElementAsync(IDeviceBrowserElement browserElement)
        {
            return await this._directoryLoader.RemoveElementFromDirectory(browserElement);
        }

        public async Task<bool> CreateNewChildDirectoryAsync(string directoryName)
        {
            return await this._directoryLoader.CreateNewChildDirectoryAsync(ElementPath + "\\" + directoryName);
        }

        public async Task<string> AddNewChildFileAsync(byte[] file, string name, string extension)
        {
            return await this._directoryLoader.CreateNewChildFileAsync(file, Name, name, extension);
        }


        #region Overrides of BrowserElementBase

        public override void SetDataProvider(IDataProvider dataProvider)
        {
            this.BrowserElementsInDirectory?.ForEach((element => element.SetDataProvider(dataProvider)));
            this._directoryLoader.SetDataProviderConnection(dataProvider);
            base.SetDataProvider(dataProvider);
        }

        #endregion

        #endregion
    }
}
