using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDriver: IDeviceContextConsumer
    {
        Task<List<string>> GetDirectoryByPath(string directoryPath);
        Task<bool> CreateDirectory(string directoryPath);
        Task<bool> DeleteElement(string path);
        Task<string> WriteFile(byte[] fileData, string directoryPath,string fileName);
        Task<byte[]> ReadFile(string directoryPath, string fileName);
    }
}