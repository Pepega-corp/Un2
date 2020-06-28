using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class CommandSender : ICommandSender
    {
        public DeviceContext DeviceContext { get; set; }

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

            await DeviceContext.DataProviderContainer.DataProvider.WriteMultipleRegistersAsync(0x5000, Extensions.ByteArrayToUshortArray(bCmd), "SetCmdFileDriver");
        }
    }
}
