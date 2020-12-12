using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public abstract class Operator
    {
        private const ushort CMD_ADDRESS = 0x5000;
        private const ushort STATE_ADDRESS = 0x5100;
        private const ushort WORDS_LENGTH = 64;
        private const ushort DATA_ADDRESS = 0x5200;

        private IDataProvider _dataProvider;

        public int LastCommandStatus { get; private set; }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        protected async Task<string> ReadDataString(string command)
        {
            var data = await ReadData(command);
            var result = GetDataString(data);
            return result;
        }

        protected async Task<ushort[]> ReadData(string command)
        {
            await SetCommand(command);
            var stateStrings = await this.ReadCommandStateStrings();
            if (stateStrings != null && stateStrings.Length >= 6)
            {
                var dataLen = Convert.ToUInt16(stateStrings[5]);
                return await ReadDataCommand(dataLen);
            }
            
            if(this.LastCommandStatus != 0)
                throw new FileOperationException(LastCommandStatus);

            return new ushort[] { };
        }

        private async Task SetCommand(string command)
        {
            var cmdStr = command;
            var cmdChar = cmdStr.ToCharArray();
            var bCmd = new byte[cmdChar.Length + 1];
            for (var i = 0; i < bCmd.Length; i++)
            {
                if (i >= cmdChar.Length)
                {
                    bCmd[i] = (byte)'\0';
                }
                else
                {
                    bCmd[i] = (byte)cmdChar[i];
                }
            }

            await _dataProvider.WriteMultipleRegistersAsync(CMD_ADDRESS, bCmd.ByteArrayToUshortArray(), "SetCmdFileDriver");
        }

        private async Task<string[]> ReadCommandStateStrings()
        {
            for (var i = 0; i < 4; i++)
            {
                var ushortQueryResult = await this._dataProvider.ReadHoldingResgistersAsync(STATE_ADDRESS, WORDS_LENGTH, "ReadStateCmdFileDriver");
                var stringResult = GetStatesStrings(ushortQueryResult.Result);
                if (ushortQueryResult.IsSuccessful && CheckState(stringResult))
                {
                    return stringResult;
                }
            }
            return null;
        }

        private bool CheckState(string[] states)
        {
            if (int.TryParse(states[1], out var res))
            {
                LastCommandStatus = res;
                return true;
            }
            else
            {
                LastCommandStatus = 255;
            }

            return false;
        }

        private string[] GetStatesStrings(ushort[] stateUshorts)
        {
            var listChar = new List<char>();
            var readedBytes = stateUshorts.UshortArrayToByteArray(false);
            foreach (var b in readedBytes)
            {
                if ((char)b != '\0')
                {
                    listChar.Add((char)b);
                }
                else break;
            }
            return new string(listChar.ToArray()).Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private async Task<ushort[]> ReadDataCommand()
        {
            return await this.ReadDataCommand(0x400);
        }

        private async Task<ushort[]> ReadDataCommand(ushort dataLen)
        {
            var result = await this._dataProvider.ReadHoldingResgistersAsync(DATA_ADDRESS, dataLen, "ReadDataFileDriver");
            if (result.IsSuccessful)
            {
                return result.Result;
            }

            throw new FileOperationException(255);
        }

        private string GetDataString(ushort[] readUshorts)
        {
            var readBytes = readUshorts.UshortArrayToByteArray(false);
            var chars = new List<char>();
            foreach (byte b in readBytes)
            {
                if (b != '\0')
                {
                    chars.Add((char)b);
                }
                else
                {
                    break;
                }
            }
            return new string(chars.ToArray());
        }

        protected async Task WriteData(byte[] writeData, string command)
        {
            var result = await this._dataProvider.WriteMultipleRegistersAsync(DATA_ADDRESS,
                writeData.ByteArrayToUshortArray(), "WriteDataFileDriver");

            if(!result.IsSuccessful)
                throw new FileOperationException(255);

            await SetCommand(command);
            await this.ReadCommandStateStrings();

            if (this.LastCommandStatus != 0)
            {
                throw new FileOperationException(this.LastCommandStatus);
            }
        }
    }
}