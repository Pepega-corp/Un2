using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface ICommandSender : IDeviceContextConsumer
    {
        Task SetCommand(string command);
    }
}