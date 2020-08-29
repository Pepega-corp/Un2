using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.FileOperations.Operators;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class FileDriver : IFileDriver
    {
        private readonly DirectoryOperator _directoryOperator;
        private readonly SessionNumberOperator _sessionNumberOperator;
        private readonly FileOpenOperator _fileOpenOperator;
        private readonly FileReadDataOperator _fileReadDataOperator;
        private readonly FileCloseOperator _fileCloseOperator;
        private readonly Dictionary<int, string> _openedFiles = new Dictionary<int, string>();

        public FileDriver(DirectoryOperator directoryOperator, SessionNumberOperator sessionNumberOperator, 
            FileOpenOperator fileOpenOperator, FileReadDataOperator fileReadDataOperator, FileCloseOperator fileCloseOperator)
        {
            this._directoryOperator = directoryOperator;
            this._sessionNumberOperator = sessionNumberOperator;
            this._fileOpenOperator = fileOpenOperator;
            this._fileReadDataOperator = fileReadDataOperator;
            this._fileCloseOperator = fileCloseOperator;
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._directoryOperator.SetDataProvider(dataProvider);
            this._sessionNumberOperator.SetDataProvider(dataProvider);
            this._fileOpenOperator.SetDataProvider(dataProvider);
            this._fileReadDataOperator.SetDataProvider(dataProvider);
            this._fileCloseOperator.SetDataProvider(dataProvider);
        }

        public async Task<byte[]> ReadFile(string fileName)
        {
            if (this._openedFiles.ContainsValue(fileName))
            {
                var openedFiles = this._openedFiles.Where(o => o.Value == fileName).ToList();
                foreach (var openFileInfo in openedFiles)
                {
                    await this._fileCloseOperator.CloseFile(openFileInfo.Key);
                    this._openedFiles.Remove(openFileInfo.Key);
                }
            }

            if (this._directoryOperator.Directory == string.Empty)
            {
                await this._directoryOperator.ReadDirectory();
                await this._sessionNumberOperator.ReadSessionNumber();
            }
            var password = this._sessionNumberOperator.GetPassword();
            var descriptor = await this._fileOpenOperator.OpenFile(fileName, this._directoryOperator.Directory,
                FileAccess.READ_FILE, password);

            this._openedFiles.Add(descriptor, fileName);

            var fileBytes = await this._fileReadDataOperator.ReadFileData(descriptor);
            await this._fileCloseOperator.CloseFile(descriptor);

            return fileBytes;

        }

        public async Task WriteFile(byte[] fileData, string fileName)
        {

        }

        public async Task<bool> CreateDirectory(string directoryPath)
        {
            return await this._directoryOperator.CreateDirectory(directoryPath);
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            return false;
        }
    }
}
