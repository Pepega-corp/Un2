using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class FileCloseOperator : Operator
    {
        private const string FILECLOSE_CMD_PATTERN = "FILECLOSE {0}";

        public async Task CloseFile(int descriptor)
        {
            await ReadData(string.Format(FILECLOSE_CMD_PATTERN, descriptor));
        }
    }
}
