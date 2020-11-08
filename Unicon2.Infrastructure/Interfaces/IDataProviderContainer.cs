using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IDataProviderContainer
    {
        Result<IDataProvider> DataProvider { get; set; }
    }
}