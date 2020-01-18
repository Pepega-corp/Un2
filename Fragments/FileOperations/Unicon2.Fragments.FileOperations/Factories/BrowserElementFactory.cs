using System;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Model.Loaders;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.FileOperations.Factories
{
    public class BrowserElementFactory : IBrowserElementFactory
    {
        private readonly IFileDriver _fileDriver;
        private IDataProvider _dataProvider;

        public BrowserElementFactory(IFileDriver fileDriver)
        {
            this._fileDriver = fileDriver;
        }

        public IDeviceDirectory CreateRootDeviceDirectoryBrowserElement()
        {
            return this.CreateDeviceDirectoryBrowserElement("0:", null);
        }

        public IDeviceDirectory CreateDeviceDirectoryBrowserElement(string path, IDeviceDirectory parentDeviceDirectory)
        {
            this._fileDriver.SetDataProvider(this._dataProvider);
            string[] elementStrings = path.Split(';');
            try
            {
                IDeviceDirectory deviceDirectory = new DeviceDirectory(path, new DirectoryLoader(this, this._fileDriver), elementStrings[0], parentDeviceDirectory);
                return deviceDirectory;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IDeviceFile CreateDeviceFileBrowserElement(string path, IDeviceDirectory parentDeviceDirectory)
        {
            this._fileDriver.SetDataProvider(this._dataProvider);
            string[] elementStrings = path.Split(';');
            IDeviceFile deviceFile = new DeviceFile(path, new FileLoader(this, this._fileDriver), elementStrings[0], parentDeviceDirectory);
            deviceFile.SetDataProvider(this._dataProvider);
            return deviceFile;
        }

        public IDeviceBrowserElement CreateBrowserElement(string path, IDeviceDirectory parentDeviceDirectory)
        {
            string[] elementStrings = path.Split(';');
            if (elementStrings[1].Contains("16"))
            {
                return this.CreateDeviceDirectoryBrowserElement(path, parentDeviceDirectory);
            }
            if (elementStrings[1].Contains("32"))
            {
                return this.CreateDeviceFileBrowserElement(path, parentDeviceDirectory);
            }
            return null;
        }

        public void SetConnectionProvider(object dataProvider)
        {
            this._dataProvider = dataProvider as IDataProvider;
        }
    }
}
