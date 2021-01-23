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

        public async Task WriteData(int descriptor, byte[] fileData)
        {
            //string file = string.Empty;
            //foreach (var b in fileData)
            //{
            //    file += b.ToString("d") + "\n";
            //}
            //File.WriteAllText("D://1.txt", file);
            var count = fileData.Length / MAX_LEN;
            var residue = fileData.Length % MAX_LEN;
            for (var counter = 0; counter < count; counter++)
            {
                byte[] writeData = fileData.Skip(MAX_LEN * counter).Take(MAX_LEN).ToArray();
                await Write(descriptor, writeData);
            }

            if (residue != 0)
            {
                var writeData = fileData.Skip(MAX_LEN * count).Take(residue).ToArray();
                await Write(descriptor, writeData);
            }
        }

        private async Task Write(int descriptor, byte[] writeData)
        {
            var crc = CRC16.CalcCrcFast(writeData, writeData.Length);
            byte[] crcBytes = crc.UshortToBytes();
            crc = Extensions.TwoBytesToUshort(crcBytes[1], crcBytes[0]);
            await WriteData(writeData, $"FILEWRITE {descriptor};{MAX_LEN};{crc}");
        }
    }
}
