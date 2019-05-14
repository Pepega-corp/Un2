using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDataReader:IDataProviderContaining
    {
        Task<byte[]> GetDataBytes(int dataLenght=256);
    }
}