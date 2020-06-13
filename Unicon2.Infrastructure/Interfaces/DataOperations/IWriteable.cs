using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Interfaces.DataOperations
{
    public interface IWriteable : IDataProviderContainer
    {
        Task<bool> Write();
    }
}