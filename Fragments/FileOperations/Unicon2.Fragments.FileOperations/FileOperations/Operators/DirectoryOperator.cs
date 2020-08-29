using System;
using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class DirectoryOperator : Operator
    {
        private const string DIRECTORY_CMD = "GETDIR";
        public string Directory { get; private set; } = string.Empty;

        public async Task ReadDirectory()
        {
            this.Directory = await ReadDataString(DIRECTORY_CMD);
        }

        public async Task<bool> CreateDirectory(string directoryPath)
        {
            await ReadData("CREATEDIR " + directoryPath);
            return LastCommandStatus == 0;
        } 
    }
}