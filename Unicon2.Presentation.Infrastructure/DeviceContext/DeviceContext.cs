using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Infrastructure.DeviceContext
{
    public class DeviceContext
    {
        public DeviceContext(IDeviceMemory deviceMemory, IDeviceEventsDispatcher deviceEventsDispatcher,
            string deviceName, IDataProviderContaining dataProviderContaining, IDeviceSharedResources deviceSharedResources)
        {
            DeviceMemory = deviceMemory;
            DeviceEventsDispatcher = deviceEventsDispatcher;
            DeviceName = deviceName;
            DataProviderContaining = dataProviderContaining;
            DeviceSharedResources = deviceSharedResources;
        }

        public IDeviceMemory DeviceMemory { get; }
        public IDeviceEventsDispatcher DeviceEventsDispatcher { get; }
        public string DeviceName { get; }
        public IDataProviderContaining DataProviderContaining { get; }
		public IDeviceSharedResources DeviceSharedResources { get; }
    }
}