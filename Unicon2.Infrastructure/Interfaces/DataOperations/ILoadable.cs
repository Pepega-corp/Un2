using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Interfaces.DataOperations
{
    public interface ILoadable : IDataProviderContainer
    {
        Task Load();
    }
}