using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public abstract class Operator : ICommandSender, ICommandStateReader, ICommandDataReader
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

        public async Task SetCommand(string command)
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

        public async Task<string[]> ReadCommandStateStrings()
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

            int res;
            if (int.TryParse(states[1], out res))
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

        public async Task<ushort[]> ReadData()
        {
            var result = await this._dataProvider.ReadHoldingResgistersAsync(STATE_ADDRESS, WORDS_LENGTH, "ReadStateCmdFileDriver");
            if (result.IsSuccessful)
            {
                return result.Result;
            }

            throw new FileOperationException(255);
        }
    }
}