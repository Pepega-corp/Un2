using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface ICommandStateReader
    {
        Task<string[]> ReadCommandStateStrings();
        int LastCommandStatus { get; }
    }
}