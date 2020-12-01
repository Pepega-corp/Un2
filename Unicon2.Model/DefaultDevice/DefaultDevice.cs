using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Model.Connection;

namespace Unicon2.Model.DefaultDevice
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultDevice : Disposable, IDevice
    {

        public DefaultDevice()
        {
            ConnectionState = new DeviceConnectionState();
            DeviceFragments = new List<IDeviceFragment>();
            DeviceSharedResources = new DeviceSharedResources();
        }

        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public IConnectionState ConnectionState { get; set; }
        [JsonProperty] public IDeviceLogger DeviceLogger { get; set; }
        [JsonProperty] public IDeviceConnection DeviceConnection { get; private set; }

        [JsonProperty] public IEnumerable<IDeviceFragment> DeviceFragments { get; set; }
        [JsonProperty] public IDeviceSharedResources DeviceSharedResources { get; set; }
        [JsonProperty] public string DeviceSignature { get; set; }

        public IDeviceMemory DeviceMemory { get; set; }
        [JsonProperty] public DeviceMetaInfo DeviceMetaInfo { get; set; }



        public void InitializeConnection(IDeviceConnection deviceConnection)
        {
            DeviceConnection = deviceConnection;
            StaticContainer.Container.Resolve<ILogService>().AddLogger(DeviceLogger, Name);
            
            if (deviceConnection is IDataProvider dataProvider)
            {
                DataProvider = Result<IDataProvider>.Create(dataProvider,true);
            }
            else
            {
                DataProvider = Result<IDataProvider>.Create(false);
            }
                
        }

        protected override void OnDisposing()
        {
            StaticContainer.Container.Resolve<ILogService>().DeleteLogger(DeviceLogger);
            DeviceConnection?.Dispose();
            base.OnDisposing();
        }

        public Result<IDataProvider> DataProvider { get; set; } = Result<IDataProvider>.Create(false);
    }

}