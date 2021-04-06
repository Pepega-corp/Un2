using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDataReader : IDeviceContextConsumer
    {
        Task<byte[]> GetDataBytes(int dataLenght=256);
    }
}