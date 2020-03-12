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
            this.ConnectionState = connectionState;
            this._logService = logService;
            this.DeviceFragments = new List<IDeviceFragment>();
            this.DeviceSharedResources = deviceSharedResources;
        }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public IConnectionState ConnectionState { get; set; }
        [JsonProperty]
        public IDeviceLogger DeviceLogger { get; set; }
        public IDeviceConnection DeviceConnection
        {
            get { return this._deviceConnection; }
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
            this._deviceConnection = deviceConnection;
            this._logService.AddLogger(this.DeviceLogger, this.Name);
            this.ConnectionState.Initialize(deviceConnection, this.DeviceLogger);
            if (deviceConnection is IDataProvider)
            {
                foreach (IDeviceFragment fragment in this.DeviceFragments)
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
            this._logService?.DeleteLogger(this.DeviceLogger);
            this._deviceConnection?.Dispose();
            base.OnDisposing();
        }
    }


}