using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

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
        
        private Result<IDataProvider> _providerResult;
        private IDataProvider _dataProvider;
        private Dictionary<string, OpenedFileInfo> _openedFiles = new Dictionary<string, OpenedFileInfo>();
        private string _workDirectory;

        private class OpenedFileInfo
        {
            public int Descriptor;
            public string FileName;
            public FileAccess Access;
            // public string Password;
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
            var fileInfo = new OpenedFileInfo();
            if (_openedFiles.ContainsKey(fileName))
            {
                fileInfo = _openedFiles[fileName];
                if (fileInfo.Access != FileAccess.ReadFile)
                {
                    MessageBox.Show($"Can't read file {fileName} because file is writing!");
                    return null;
                }
            }
            else
            {
                fileInfo.Access = FileAccess.ReadFile;
                fileInfo.FileName = fileName;
                fileInfo.SessionNumber = await this.ReadSessionNumber();
                await OpenFile(fileInfo);
                _openedFiles.Add(fileName, fileInfo);
            }
            
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

                    data.AddRange(await ReadDataCommand((ushort)(dataLen/2)));
                }
                else
                {
                    break;
                }
            }

            throw new FileOperationException(LastCommandStatus);
        }

        public Task WriteFile(byte[] fileData, string fileName, ushort wordsDataLen = 64)
        {
            throw new System.NotImplementedException();
        }
        
        public Task<bool> DeleteFile(string fileName)
        {
            throw new System.NotImplementedException();
        }
        
        private async Task<string> ReadDirectory()
        {
            return await ReadDataString(DIRECTORY_CMD);
        }
        
        private async Task OpenFile(OpenedFileInfo fileInfo)
        {
            var password = fileInfo.GetPassword();
            var accessStr = fileInfo.Access == FileAccess.ReadFile ? "1" : "10";
            var descriptorStr =
                await ReadDataString(string.Format(FILEOPEN_PATTERN, password, accessStr, _workDirectory, fileInfo.FileName));
            fileInfo.Descriptor = Convert.ToInt32(descriptorStr);
        }
        
        private async Task CloseFile(string fileName)
        {
            if (_openedFiles.ContainsKey(fileName))
            {
                var fileInfo = _openedFiles[fileName];
                // TODO close file
                this._openedFiles.Remove(fileName);
            }
        }

        private async Task<int> ReadSessionNumber()
        {
            var str = await ReadDataString(GETNUM_CMD);
            
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
            
            return Convert.ToUInt16(fixedStr);
        }
        
        #region Common commands

        private async Task<string> ReadDataString(string cmd)
        {
            await SendCommand(cmd);
            var stateStrings = await ReadCommandStateStrings();
            if (LastCommandStatus != 0 && stateStrings != null && stateStrings.Length >= 6)
            {
                var dataLen = Convert.ToUInt16(stateStrings[5]);
                var data = await ReadDataCommand(dataLen);
                return GetDataString(data);
            }
            
            throw new FileOperationException(this.LastCommandStatus != 0 ? LastCommandStatus : 255);
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
                    if (CheckState(stateStrings))
                    {
                        return stateStrings;
                    }
                }
            }

            throw new FileOperationException(255);
        }
        
        private async Task<ushort[]> ReadDataCommand(ushort dataLen)
        {
            var dataResult = await _dataProvider.ReadHoldingResgistersAsync(DATA_ADDRESS, dataLen, "ReadDataFileDriver");

            if (dataResult.IsSuccessful)
            {
                return dataResult.Result;
            }

            throw new FileOperationException(255);
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
        
        private bool CheckState(string[] states)
        {
            if (states == null || states.Length == 0)
                return false;

            if (int.TryParse(states[1], out var res))
            {
                LastCommandStatus = res;
                return LastCommandStatus == 0;
            }

            this.LastCommandStatus = 255;

            return false;
        }
        
        private string GetDataString(ushort[] readUshorts)
        {
            var readBytes = readUshorts.UshortArrayToByteArray(false);
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
        
        #endregion

        public Task<bool> CreateDirectory(string directoryPath)
        {
            throw new System.NotImplementedException();
        }
    }
}