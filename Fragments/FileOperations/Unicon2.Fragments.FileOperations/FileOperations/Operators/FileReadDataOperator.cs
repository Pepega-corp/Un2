using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class FileReadDataOperator : Operator
    {
        private const string FILEREAD_CMD_PATTERN = "FILEREAD {0};{1}";
        private const ushort FILE_DATA_LEN = 2048;

        public async Task<byte[]> ReadFileData(int descriptor)
        {
            var dataWords = await ReadData(string.Format(FILEREAD_CMD_PATTERN, descriptor, FILE_DATA_LEN));

            return dataWords.UshortArrayToByteArray(false);
        }

        protected override async Task<ushort[]> ReadDataCommand(ushort bytesDataLen) //dataLen in bytes
        {
            var wordsDataLen = bytesDataLen / 2;
            if (wordsDataLen > WORDS_LENGTH)
            {
                var count = wordsDataLen / WORDS_LENGTH;
                var residue = wordsDataLen % WORDS_LENGTH;
                var retWords = new List<ushort>();
                int startAddress = DATA_ADDRESS;
                for (var i = 0; i < count; i++)
                {
                    var result = await ReadDataCommand((ushort)startAddress, WORDS_LENGTH);
                    retWords.AddRange(result);
                    startAddress += i * WORDS_LENGTH;
                }
                if (residue > 0)
                {
                    var result = await ReadDataCommand((ushort)startAddress, (ushort)residue);
                    retWords.AddRange(result);
                }

                return retWords.ToArray();
            }
            else
            {
                return await base.ReadDataCommand((ushort)wordsDataLen);
            }
        }


    }
}
