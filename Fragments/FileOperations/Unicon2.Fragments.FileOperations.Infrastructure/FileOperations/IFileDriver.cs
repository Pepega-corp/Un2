using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDriver
    {
        Task<List<string>> GetDirectoryByPath(string directoryPath);
        Task<bool> CreateDirectory(string directoryPath);
        Task<bool> DeleteElement(string path);
        Task<string> WriteFile(byte[] fileData,string directoryPath,string fileName);
        
    }
}