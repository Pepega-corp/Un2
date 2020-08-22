using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class DirectoryOperator : Operator
    {
        private const string DIRECTORY_CMD = "GETDIR";
        public string Directory { get; private set; }

        public async Task ReadDirectory()
        {
            await this.SetCommand(DIRECTORY_CMD);

            var states = await ReadCommandStateStrings();

            if (states != null)
            {
                
            }
            else
            {
                throw new FileOperationException(LastCommandStatus);
            }

           // Directory = (await _dataReader.ReadData((ushort)dataWordsLen)).Replace("/", "");
        }

        public async Task<bool> CreateDirectory(string directoryPath)
        {
            await SetCommand("CREATEDIR " + directoryPath);
            await ReadCommandStateStrings();
            return LastCommandStatus == 0;
        } 
    }
}