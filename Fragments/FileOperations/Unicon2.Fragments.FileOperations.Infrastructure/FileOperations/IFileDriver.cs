using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDriver : IDataProviderSetter
    {
        Task<byte[]> ReadFile(string fileName, ushort maxWordsLen = 64);
        Task WriteFile(byte[] fileData, string fileName, ushort wordsDataLen = 64);
        Task<bool> CreateDirectory(string directoryPath);
        Task<bool> DeleteFile(string fileName);
    }
}