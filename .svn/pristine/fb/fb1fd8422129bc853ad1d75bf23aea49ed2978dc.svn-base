using System;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.FileOperations.Model.Loaders
{
    public class FileLoader : IFileLoader
    {
        private readonly IBrowserElementFactory _browserElementFactory;
        private readonly IFileDriver _fileDriver;
        private IDataProvider _dataProvider;

        public FileLoader(IBrowserElementFactory browserElementFactory, IFileDriver fileDriver)
        {
            this._browserElementFactory = browserElementFactory;
            this._fileDriver = fileDriver;
        }


        public byte[] LoadFileData(string filePath)
        {
            throw new NotImplementedException();
        }

        public void SetDataProvider(object dataprovider)
        {
            this._dataProvider = dataprovider as IDataProvider;
        }
    }
}
