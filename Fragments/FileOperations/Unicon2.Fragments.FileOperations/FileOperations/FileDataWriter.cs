using System;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class FileDataWriter : IFileDataWriter
    {
        private readonly ICommandSender _commandSender;
        private readonly ICommandStateReader _commandStateReader;
        private DeviceContext _deviceContext;

        public FileDataWriter(ICommandSender commandSender, ICommandStateReader commandStateReader)
        {
            this._commandSender = commandSender;
            this._commandStateReader = commandStateReader;
        }

        public DeviceContext DeviceContext
        {
            get => _deviceContext;
            set
            {
                _deviceContext = value;
                this._commandSender.DeviceContext = _deviceContext;
                this._commandStateReader.DeviceContext = _deviceContext;
            }
        }

        public async Task<bool> WriteData(byte[] bytesToWrite, string descriptor)
        {

            ushort[] ushortsToWrite = bytesToWrite.ByteArrayToUshortArray();
            int lenght = ushortsToWrite.Length;
            int iterator = 64;
            if (lenght < 64)
            {
                iterator = lenght;
            }

            int currentCount = 0;
            int curLenght = 0;
            int allCount = bytesToWrite.Length / 128 + 1;
            int lastLenght = bytesToWrite.Length % 128;
            if (lastLenght == 0) allCount--;

            while (allCount > currentCount)
            {
                curLenght = bytesToWrite.Skip(128 * currentCount).ToArray().Length >= 128
                    ? 128
                    : lastLenght;
                byte[] writeData = new byte[curLenght];
                byte[] buf = bytesToWrite.Skip(128 * currentCount).Take(curLenght).ToArray();
                for (int i = 0; i < buf.Length; i++)
                {
                    writeData[i] = buf[i];
                }
                await this.DeviceContext.DataProviderContainer.DataProvider.WriteMultipleRegistersAsync(0x5200,
                    writeData.ByteArrayToUshortArray(), "writingFile");
                ushort crc = CRC16.CalcCrcFast(buf, buf.Length);
                byte[] crcBytes = crc.UshortToBytes();
                crc = Extensions.ToUshort(crcBytes[1], crcBytes[0]);
                await this._commandSender.SetCommand("FILEWRITE " + Convert.ToUInt16(descriptor) + ";" + curLenght + ";" + crc);
                string[] stateStrings = await this._commandStateReader.ReadCommandStateStrings();
                currentCount++;
            }

            //for (int i = 0; i < lenght; i += iterator)
            //{
            //    int lenghtToTake = iterator;
            //    if ((lenght - iterator - i) < 0)
            //    {
            //        lenghtToTake = lenght - i;
            //    }
            //    ushort[] dataToWrite = ushortsToWrite.Skip(i * iterator).Take(lenghtToTake).ToArray();

            //    ushort crc;
            //    byte[] b = bytesToWrite.Skip(i * 2).Take(lenghtToTake * 2).ToArray();
            //    crc = CRC16.CalcCrcFast(b, lenghtToTake);
            //    byte[] crcBytes = crc.UshortToBytes();
            //    crc = Extensions.ToUshort(crcBytes[1], crcBytes[0]);


            //    error = Convert.ToUInt16(stateStrings[1]);
            //    if (error != 0)
            //    {
            //        return _messagesDictionary[error];
            //    }
            //}


            return true;
        }
    }
}
