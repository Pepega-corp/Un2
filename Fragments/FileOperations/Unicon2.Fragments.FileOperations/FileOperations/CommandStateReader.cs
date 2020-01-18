using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class CommandStateReader : ICommandStateReader
    {
        private IDataProvider _dataProvider;
        private int _lastCommandStatus;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<string[]> ReadCommandStateStrings()
        {
            IQueryResult<ushort[]> ushortQueryResult = null;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    ushortQueryResult = await _dataProvider.ReadHoldingResgistersAsync(0x5100, 64, "ReadStateCmdFileDriver");
                    if (ushortQueryResult.IsSuccessful)
                    {
                        CheckState(GetStatesStrings(ushortQueryResult.Result));
                    }
                    return GetStatesStrings(ushortQueryResult.Result);
                }
                catch (Exception e)
                {
                    continue;
                }

            }
            return null;

        }

        public int LastCommandStatus
        {
            get { return _lastCommandStatus; }
        }

        private bool CheckState(string[] states)
        {
            
                int res;
                if (int.TryParse(states[1], out res))
                {
                    _lastCommandStatus = res;
                    return true;
                }
                else
                {
                    _lastCommandStatus = 255;
                }
       

            return false;
        }



        private string[] GetStatesStrings(ushort[] stateUshorts)
        {
            List<char> listChar = new List<char>();
            byte[] readedBytes = Extensions.UshortArrayToByteArray(stateUshorts, false);
            foreach (byte b in readedBytes)
            {
                if ((char)b != '\0')
                {
                    listChar.Add((char)b);
                }
                else break;
            }
            return new string(listChar.ToArray()).Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
