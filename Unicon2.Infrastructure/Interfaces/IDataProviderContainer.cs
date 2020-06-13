using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IDataProviderContainer
    {
        IDataProvider DataProvider { get; set; }
    }
}