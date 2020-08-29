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
    }
}
