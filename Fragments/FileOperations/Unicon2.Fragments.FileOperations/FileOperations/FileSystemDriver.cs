using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class FileSystemDriver : IFileDriver
    {
        protected const ushort CMD_ADDRESS = 0x5000;
        protected const ushort STATE_ADDRESS = 0x5100;
        protected const ushort DATA_ADDRESS = 0x5200;
        
        private const string DIRECTORY_CMD = "GETDIR";
        private const string GETNUM_CMD = "GETNUMBER";
        private const string FILEOPEN_PATTERN = "FILEOPEN {0};{1};{2}{3}";
        private const string FILEREAD_CMD_PATTERN = "FILEREAD {0};{1}";
        private const string FILECLOSE_CMD_PATTERN = "FILECLOSE {0}";

        private Result<IDataProvider> _providerResult;
        private IDataProvider _dataProvider;
        private Dictionary<string, OpenedFileInfo> _openedFiles = new Dictionary<string, OpenedFileInfo>();
        private string _workDirectory;

        private class OpenedFileInfo
        {
            public int Descriptor;
            public string FileName;
            public FileAccess Access;
            public int SessionNumber;
            

            public string GetPassword()
            {
                var pass = "АААА"; //русские буквы А
                var ascii = Encoding.GetEncoding("windows-1251");
                var asciiBytes = ascii.GetBytes(pass);
                var passCode = string.Empty;
                foreach (var c in asciiBytes)
                {
                    SessionNumber = (byte)((SessionNumber * 99 + 53) % 256);
                    passCode += ((byte)(c + SessionNumber)).ToString("X2");
                }
                return passCode;
            }
        }

        public int LastCommandStatus { get; private set; }

        public async void SetDataProvider(IDataProviderContainer dataProviderContainer)
        {
            _providerResult = dataProviderContainer.DataProvider;
            _dataProvider = dataProviderContainer.DataProvider.Item;
            if (_providerResult.IsSuccess)
            {
                _workDirectory = await ReadDirectory();
            }
        }

        public async Task<byte[]> ReadFile(string fileName, ushort maxWordsLen = 64)
        {
            if (!this._providerResult.IsSuccess)
            {
                return null;
            }

            if (string.IsNullOrEmpty(this._workDirectory))
            {
                this._workDirectory = await this.ReadDirectory();
            }

            var fileInfo = new OpenedFileInfo();
            if (_openedFiles.ContainsKey(fileName))
            {
                fileInfo = _openedFiles[fileName];
                await CloseFile(fileInfo);
            }
            
            fileInfo.Access = FileAccess.ReadFile;
            fileInfo.FileName = fileName;
            fileInfo.SessionNumber = await this.ReadSessionNumber();
            await OpenFile(fileInfo);
            
            var dataFile = await ReadData(fileInfo, maxWordsLen);
            await CloseFile(fileInfo);
            return dataFile;
        }

        public async Task WriteFile(byte[] fileData, string fileName, ushort wordsDataLen = 64)
        {
            if (!this._providerResult.IsSuccess)
            {
                return;
            }

            if (string.IsNullOrEmpty(this._workDirectory))
            {
                this._workDirectory = await this.ReadDirectory();
            }

            var fileInfo = new OpenedFileInfo();
            if (_openedFiles.ContainsKey(fileName))
            {
                fileInfo = _openedFiles[fileName];
                await CloseFile(fileInfo);
            }

            fileInfo.Access = FileAccess.WriteFile;
            fileInfo.FileName = fileName;

            await OpenFile(fileInfo);
            
            var dataLength = wordsDataLen * 2;
            var count = fileData.Length / dataLength;
            var residue = fileData.Length % dataLength;
            for (var counter = 0; counter < count; counter++)
            {
                byte[] writeData = fileData.Skip(dataLength * counter).Take(dataLength).ToArray();
                await WriteData(fileInfo.Descriptor, writeData, dataLength);
            }

            if (residue != 0)
            {
                var writeData = fileData.Skip(dataLength * count).Take(residue).ToArray();
                await WriteData(fileInfo.Descriptor, writeData, residue);
            }


            await CloseFile(fileInfo);
        }

        private async Task WriteData(int descriptor, byte[] writeData, int dataLength)
        {
            var tryCounter = 0;
            do
            {
                var crc = CRC16.CalcCrcFast(writeData, writeData.Length);
                byte[] crcBytes = crc.UshortToBytes();
                crc = Extensions.TwoBytesToUshort(crcBytes[1], crcBytes[0]);
                var command = $"FILEWRITE {descriptor};{dataLength};{crc}";
                var res = await _dataProvider.WriteMultipleRegistersAsync(DATA_ADDRESS,
                    writeData.ByteArrayToUshortArray(), "WriteDataFileDriver");
                if (!res.IsSuccessful)
                    throw new FileOperationException(255);
                await this.SendCommand(command);
                var states = await this.ReadCommandStateStrings();
                this.SetLastCommandStatus(states);
                if (this.LastCommandStatus != 0)
                {
                    tryCounter++;
                }
                else
                {
                    return;
                }
            }
            while (tryCounter < 5);

            throw new FileOperationException(this.LastCommandStatus);
        }
        
        public async Task<bool> DeleteFile(string fileName)
        {
            if (!this._providerResult.IsSuccess)
            {
                return false;
            }

            return await Task.FromResult(false);
        }
        
        private async Task<string> ReadDirectory()
        {
            return await ReadDataString(DIRECTORY_CMD);
        }
        
        private async Task OpenFile(OpenedFileInfo fileInfo)
        {
            var counter = 0;
            do
            {
                fileInfo.SessionNumber = await this.ReadSessionNumber();
                var password = fileInfo.GetPassword();
                var accessStr = fileInfo.Access == FileAccess.ReadFile ? "1" : "10";
                var descriptorStr = await ReadDataString(string.Format(FILEOPEN_PATTERN, password, accessStr, _workDirectory, fileInfo.FileName));
                if (string.IsNullOrEmpty(descriptorStr))
                {
                    counter++;
                }
                else
                {
                    fileInfo.Descriptor = Convert.ToInt32(descriptorStr);
                    _openedFiles.Add(fileInfo.FileName, fileInfo);
                    return;
                }
            }
            while (counter < 5);

            throw new FileOperationException(this.LastCommandStatus);
        }
        
        private async Task CloseFile(OpenedFileInfo fileInfo)
        {
            if (_openedFiles.ContainsKey(fileInfo.FileName))
            {
                await SendCommand(string.Format(FILECLOSE_CMD_PATTERN, fileInfo.Descriptor));
                var counter = 0; 
                do
                {
                    var stateStrings = await ReadCommandStateStrings();
                    if (LastCommandStatus == 0 || this.LastCommandStatus == 8)
                    {
                        this._openedFiles.Remove(fileInfo.FileName);
                        return;
                    }

                    counter++;
                }
                while (counter < 5);

                throw new FileOperationException(this.LastCommandStatus);
            }
        }

        private async Task<int> ReadSessionNumber()
        {
            var counter = 0;
            do
            {
                var str = await ReadDataString(GETNUM_CMD);
                if (str.Length > 3)
                {
                    var fixedStr = string.Empty;
                    for (var i = 0; i < 3; i++)
                    {
                        if (char.IsDigit(str[i]))
                        {
                            fixedStr += str[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                    str = fixedStr;
                }

                if (ushort.TryParse(str, out var sessionNumber) && sessionNumber < 256)
                {
                    return sessionNumber;
                }
                counter++;
            }

            while (counter < 5);

            throw new FileOperationException(this.LastCommandStatus);
        }
        
        #region Common commands

        private async Task<string> ReadDataString(string cmd)
        {
            await SendCommand(cmd);
            await Task.Delay(10);
            var stateStrings = await ReadCommandStateStrings();
            await Task.Delay(10);
            if (LastCommandStatus == 0 && stateStrings != null && stateStrings.Length >= 6)
            {
                var dataLen = Convert.ToUInt16(stateStrings[5]);
                var data = await ReadDataCommand(dataLen);
                var readBytes = data.UshortArrayToByteArray(false);
                return GetDataString(readBytes.Take(dataLen).ToArray());
            }

            return string.Empty;
        }
        
        private async Task SendCommand(string command)
        {
            var cmdStr = command;
            var cmdChar = cmdStr.ToCharArray();
            var bCmd = new byte[cmdChar.Length + 1];
            for (var i = 0; i < bCmd.Length; i++)
            {
                if (i >= cmdChar.Length)
                {
                    bCmd[i] = (byte) '\0';
                }
                else
                {
                    bCmd[i] = (byte) cmdChar[i];
                }
            }

            var result = await _dataProvider.WriteMultipleRegistersAsync(CMD_ADDRESS, bCmd.ByteArrayToUshortArray(), "SetCmdFileDriver");

            if (!result.IsSuccessful)
            {
                throw new FileOperationException(255);
            }
        }
        
        private async Task<string[]> ReadCommandStateStrings(ushort wordsDataLen = 64)
        {
            for (var i = 0; i < 4; i++)
            {
                var stateWords = await _dataProvider.ReadHoldingResgistersAsync(STATE_ADDRESS, wordsDataLen, "ReadStateCmdFileDriver");

                if (stateWords.IsSuccessful)
                {
                    var stateStrings = GetStatesStrings(stateWords.Result);
                    this.SetLastCommandStatus(stateStrings);
                    if (this.LastCommandStatus == -1)
                    {
                        continue;
                    }
                    return stateStrings;
                }
            }
            StaticContainer.Container.Resolve<ILogService>().LogMessage("Не удалось прочитать файл для программирования");
            return new string[] { };
            // throw new FileOperationException(this.LastCommandStatus == 0 ? 255 : this.LastCommandStatus);
        }
        
        private async Task<ushort[]> ReadDataCommand(ushort dataLen)
        {
            var dataResult = await _dataProvider.ReadHoldingResgistersAsync(DATA_ADDRESS, dataLen, "ReadDataFileDriver");

            if (dataResult.IsSuccessful)
            {
                return dataResult.Result;
            }

            return null;
        }
        
        private string[] GetStatesStrings(ushort[] stateUshorts)
        {
            var listChar = new List<char>();
            var readedBytes = stateUshorts.UshortArrayToByteArray(false);
            foreach (var b in readedBytes)
            {
                if ((char) b != '\0')
                {
                    listChar.Add((char) b);
                }
                else break;
            }

            return new string(listChar.ToArray()).Split(new[] {' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
        }
        
        private void SetLastCommandStatus(string[] states)
        {
            if (states != null && states.Length > 0 && int.TryParse(states[1], out var res))
            {
                this.LastCommandStatus = res;
            }
            else
            {
                LastCommandStatus = -1; // unknown state. Need to read again
            }
        }
        
        private string GetDataString(byte[] readBytes)
        {
            if (readBytes == null || readBytes.Length == 0)
            {
                return string.Empty;
            }
            
            var chars = new List<char>();
            foreach (byte b in readBytes)
            {
                if (b != '\0')
                {
                    chars.Add((char) b);
                }
                else
                {
                    break;
                }
            }

            return new string(chars.ToArray());
        }

        private async Task<byte[]> ReadData(OpenedFileInfo fileInfo, ushort maxWordsLen)
        {
            var data = new List<ushort>();
            var dataLen = ushort.MaxValue;
            while (dataLen > 0)
            {
                await SendCommand(string.Format(FILEREAD_CMD_PATTERN, fileInfo.Descriptor, maxWordsLen * 2));
                var stateStrings = await this.ReadCommandStateStrings();
                if (stateStrings != null && stateStrings.Length >= 6)
                {
                    dataLen = Convert.ToUInt16(stateStrings[5]);

                    if (dataLen == 0)
                        return data.UshortArrayToByteArray(false);

                    data.AddRange(await ReadDataCommand((ushort)(dataLen / 2)));
                }
                else
                {
                    break;
                }
            }

            throw new FileOperationException(this.LastCommandStatus != 0 ? LastCommandStatus : 255);
        }
        
        #endregion

        public async Task<bool> CreateDirectory(string directoryPath)
        {
            var str = await this.ReadDataString("CREATEDIR " + directoryPath);
            return this.LastCommandStatus == 0;
        }
    }
}