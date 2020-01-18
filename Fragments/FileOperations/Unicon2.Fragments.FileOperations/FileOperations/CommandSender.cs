using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class CommandSender : ICommandSender
    {
        private IDataProvider _dataProvider;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public async Task SetCommand(string command)
        {
            string cmdStr = command;
            char[] cmdChar = cmdStr.ToCharArray();
            byte[] bCmd = new byte[cmdChar.Length + 1];
            for (int i = 0; i < bCmd.Length; i++)
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
            await this._dataProvider.WriteMultipleRegistersAsync(0x5000, Extensions.ByteArrayToUshortArray(bCmd), "SetCmdFileDriver");
        }
    }
}
