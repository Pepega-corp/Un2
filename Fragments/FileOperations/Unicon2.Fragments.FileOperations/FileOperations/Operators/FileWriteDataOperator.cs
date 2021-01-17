using System;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class FileWriteDataOperator : Operator
    {
        private const int MAX_LEN = 246; //max data len is 246 bytes
        //Need to create writing 2048 bytes for new devices

        public async Task WriteData(int descriptor, byte[] fileData)
        {
            var count = fileData.Length / MAX_LEN;
            var residue = fileData.Length % MAX_LEN;
            if (residue != 0)
            {
                count++;
            }
            for (var counter = 0; counter < count; counter++)
            {
                var curLen = fileData.Skip(MAX_LEN * counter).ToArray().Length >= MAX_LEN
                    ? MAX_LEN
                    : residue;
                byte[] writeData = new byte[MAX_LEN];
                byte[] buf = fileData.Skip(MAX_LEN * counter).Take(curLen).ToArray();
                for (int i = 0; i < buf.Length; i++)
                {
                    writeData[i] = buf[i];
                }
                var crc = CRC16.CalcCrcFast(buf, buf.Length);
                byte[] crcBytes = crc.UshortToBytes();
                crc = Extensions.TwoBytesToUshort(crcBytes[1], crcBytes[0]);
                await WriteData(writeData, $"FILEWRITE {descriptor};{curLen};{crc}");
            }
        }
    }
}
