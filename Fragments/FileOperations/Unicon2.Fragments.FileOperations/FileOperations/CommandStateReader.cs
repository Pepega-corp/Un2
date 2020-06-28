using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class CommandStateReader : ICommandStateReader
    {
        public async Task<string[]> ReadCommandStateStrings()
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    IQueryResult<ushort[]> ushortQueryResult = await DeviceContext.DataProviderContainer.DataProvider.ReadHoldingResgistersAsync(0x5100, 64, "ReadStateCmdFileDriver");
                    if (ushortQueryResult.IsSuccessful)
                    {
                        CheckState(GetStatesStrings(ushortQueryResult.Result));
                    }
                    return GetStatesStrings(ushortQueryResult.Result);
                }
                catch
                {
                    continue;
                }

            }
            return null;
        }

        public int LastCommandStatus { get; private set; }

        public DeviceContext DeviceContext { get; set; }

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
