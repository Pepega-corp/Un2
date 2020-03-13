using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.DefaultDevice
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultDevice : Disposable, IDevice
    {
        private ILogService _logService;

        private IDeviceConnection _deviceConnection;

        public DefaultDevice(IConnectionState connectionState, ILogService logService, IDeviceSharedResources deviceSharedResources)
        {
            ConnectionState = connectionState;
            _logService = logService;
            DeviceFragments = new List<IDeviceFragment>();
            DeviceSharedResources = deviceSharedResources;
        }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public IConnectionState ConnectionState { get; set; }
        [JsonProperty]
        public IDeviceLogger DeviceLogger { get; set; }
        public IDeviceConnection DeviceConnection
        {
            get { return _deviceConnection; }
        }
        [JsonProperty]
        public IEnumerable<IDeviceFragment> DeviceFragments { get; set; }
        [JsonProperty]
        public IDeviceSharedResources DeviceSharedResources { get; set; }
        [JsonProperty]
        public string DeviceSignature { get; set; }

		public IDeviceMemory DeviceMemory { get; set; }


        public void InitializeConnection(IDeviceConnection deviceConnection)
        {
            _deviceConnection = deviceConnection;
            _logService.AddLogger(DeviceLogger, Name);
            ConnectionState.Initialize(deviceConnection, DeviceLogger);
            if (deviceConnection is IDataProvider)
            {
                foreach (IDeviceFragment fragment in DeviceFragments)
                {
                    if (fragment is IDataProviderContaining dataProviderContaining)
                    {
                        dataProviderContaining.DataProvider = ((IDataProvider) deviceConnection);
                    }
                }
            }
        }
        protected override void OnDisposing()
        {
            _logService?.DeleteLogger(DeviceLogger);
            _deviceConnection?.Dispose();
            base.OnDisposing();
        }
    }


}