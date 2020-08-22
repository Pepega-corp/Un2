using System;
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
                var dataLen = Convert.ToUInt16(states[5]);
                Directory = GetDataString(await ReadData(dataLen));
            }
            else
            {
                throw new FileOperationException(LastCommandStatus);
            }
        }

        public async Task<bool> CreateDirectory(string directoryPath)
        {
            await SetCommand("CREATEDIR " + directoryPath);
            await ReadCommandStateStrings();
            return LastCommandStatus == 0;
        } 
    }
}