using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface ICommandSender 
    {
        Task SetCommand(string command);
    }
}