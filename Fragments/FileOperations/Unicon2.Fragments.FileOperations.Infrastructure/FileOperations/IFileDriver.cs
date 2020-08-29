using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDriver : IDataProviderSetter
    {
        Task<byte[]> ReadFile(string fileName);
        Task WriteFile(byte[] fileData, string fileName);
        Task<bool> CreateDirectory(string directoryPath);
        Task<bool> DeleteFile(string fileName);

        //Task<List<string>> GetDirectoryByPath(string directoryPath);
    }
}