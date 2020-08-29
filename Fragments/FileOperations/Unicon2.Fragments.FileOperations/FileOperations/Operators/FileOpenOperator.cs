using System;
using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class FileOpenOperator : Operator
    {
        private const string FILEOPEN_PATTERN = "FILEOPEN {0};{1};{2}{3}";
        public int Descriptor { get; private set; }

        public async Task<int> OpenFile(string fileName, string directory, FileAccess access, string password)
        {
            var accessStr = access == FileAccess.READ_FILE ? "1" : "10";

            var data = await ReadDataString(string.Format(FILEOPEN_PATTERN, password, accessStr, directory, fileName));
            this.Descriptor = Convert.ToInt32(data);
            return this.Descriptor;
        }
    }
}
