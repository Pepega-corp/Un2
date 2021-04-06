using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IFileDataWriter : IDeviceContextConsumer
    {
        Task<bool> WriteData(byte[] bytesToWrite, string descriptor);
    }
}