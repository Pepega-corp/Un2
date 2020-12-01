using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface ICommandSender
    {
        Task SetCommand(string command);
    }
}