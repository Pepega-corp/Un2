using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Model
{
   public class ConnectionContainer:IDataProviderContainer
    {
        public ConnectionContainer(IDataProvider dataProvider)
        {
            DataProvider = Result<IDataProvider>.Create(dataProvider,true);
        }
        public Result<IDataProvider> DataProvider { get; set; }
    }
}
