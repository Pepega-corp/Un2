using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class FileWriteDataOperator : Operator
    {
        private const int MAX_LEN = 128; //max data len is 246 bytes
        //Need to create writing 2048 bytes for new devices

        public async Task WriteData(int descriptor, byte[] fileData, int dataLength = MAX_LEN)
        {
            var count = fileData.Length / dataLength;
            var residue = fileData.Length % dataLength;
            for (var counter = 0; counter < count; counter++)
            {
                byte[] writeData = fileData.Skip(dataLength * counter).Take(dataLength).ToArray();
                await Write(descriptor, writeData, dataLength);
            }

            if (residue != 0)
            {
                var writeData = fileData.Skip(dataLength * count).Take(residue).ToArray();
                await Write(descriptor, writeData, residue);
            }
        }

        private async Task Write(int descriptor, byte[] writeData, int dataLength)
        {
            var crc = CRC16.CalcCrcFast(writeData, writeData.Length);
            byte[] crcBytes = crc.UshortToBytes();
            crc = Extensions.TwoBytesToUshort(crcBytes[1], crcBytes[0]);
            await WriteData(writeData, $"FILEWRITE {descriptor};{dataLength};{crc}");
        }
    }
}
