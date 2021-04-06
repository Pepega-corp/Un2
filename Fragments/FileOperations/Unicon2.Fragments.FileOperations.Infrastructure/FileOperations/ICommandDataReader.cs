
using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface ICommandDataReader
    {
        Task<ushort[]> ReadData();
        Task<ushort[]> ReadData(ushort dataLen);
    }
}