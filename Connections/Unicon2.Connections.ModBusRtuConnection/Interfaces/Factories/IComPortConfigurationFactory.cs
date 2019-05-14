using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories
{
   public interface IComPortConfigurationFactory
   {
       IComPortConfiguration CreateComPortConfiguration();
   }
}
