using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IDataProviderContaining
    {
        IDataProvider DataProvider { get; set; }
    }
}