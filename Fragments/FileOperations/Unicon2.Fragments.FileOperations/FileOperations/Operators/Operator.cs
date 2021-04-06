using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public abstract class Operator
    {
        protected const ushort CMD_ADDRESS = 0x5000;
        protected const ushort STATE_ADDRESS = 0x5100;
        protected const ushort WORDS_LENGTH = 64;
        protected const ushort DATA_ADDRESS = 0x5200;

        protected IDataProviderContainer _dataProvider;

        public int LastCommandStatus { get; private set; }

        public void SetDataProvider(IDataProviderContainer dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        protected async Task<string> ReadDataString(string command)
        {
            var data = await ReadData(command);
            var result = GetDataString(data);
            return result;
        }

        protected virtual async Task<ushort[]> ReadData(string command)
        {
            await SetCommand(command);
            var stateStrings = await this.ReadCommandStateStrings();
            if (stateStrings != null && stateStrings.Length >= 6)
            {
                var dataLen = Convert.ToUInt16(stateStrings[5]);
                return await ReadDataCommand(dataLen);
            }

            if (this.LastCommandStatus != 0)
                throw new FileOperationException(LastCommandStatus);

            return new ushort[] { };
        }

        protected async Task SetCommand(string command)
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

            await _dataProvider.DataProvider.OnSuccessAsync(async (provider) =>
            {
                await provider.WriteMultipleRegistersAsync(CMD_ADDRESS, bCmd.ByteArrayToUshortArray(),
                    "SetCmdFileDriver");
                return provider;
            });
        }

        protected async Task<string[]> ReadCommandStateStrings()
        {
            for (var i = 0; i < 4; i++)
            {
                var res = await _dataProvider.DataProvider.OnSuccessAsync<string[]>(async (provider) =>
                {
                    var ushortQueryResult =
                        await provider.ReadHoldingResgistersAsync(STATE_ADDRESS, WORDS_LENGTH,
                            "ReadStateCmdFileDriver");
                    var stringResult = GetStatesStrings(ushortQueryResult.Result);
                    if (ushortQueryResult.IsSuccessful && CheckState(stringResult))
                    {
                        return stringResult;
                    }

                    return Result<string[]>.Create(false);
                });

                if (res.IsSuccess)
                {
                    return res.Item;
                }
            }

            return null;
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

        protected virtual async Task<ushort[]> ReadDataCommand(ushort dataLen)
        {
            var res = await _dataProvider.DataProvider.OnSuccessAsync<IQueryResult<ushort[]>>(async (provider) =>
                Result<IQueryResult<ushort[]>>.Create(
                    await provider.ReadHoldingResgistersAsync(DATA_ADDRESS, dataLen, "ReadDataFileDriver"), true));

            if (res.IsSuccess && res.Item.IsSuccessful)
            {
                return res.Item.Result;
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
                    chars.Add((char) b);
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

            var res = await _dataProvider.DataProvider.OnSuccessAsync(async (provider) => Result<IQueryResult>.Create(
                await provider.WriteMultipleRegistersAsync(DATA_ADDRESS,
                    writeData.ByteArrayToUshortArray(), "WriteDataFileDriver"), true));


           
            if (!res.IsSuccess||!res.Item.IsSuccessful)
                throw new FileOperationException(255);
            await SetCommand(command);
            var states = await this.ReadCommandStateStrings();
            if (!CheckState(states))
                throw new FileOperationException(this.LastCommandStatus);

        }
    }
}