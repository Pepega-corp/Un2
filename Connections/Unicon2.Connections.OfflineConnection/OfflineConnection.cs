using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Connections.OfflineConnection
{

    [JsonObject(MemberSerialization.OptIn)]
    public class OfflineConnection : Disposable, IDeviceConnection
    {
        private IDeviceLogger _currentDeviceLogger;

        public void SetLogger(IDeviceLogger currentDeviceLogger)
        {
            _currentDeviceLogger = currentDeviceLogger;
        }

        public string ConnectionName => "Offline";


        public bool TryOpenConnection(bool isThrowingException)
        {
            return true;
        }

        public Task<Result> TryOpenConnectionAsync(IDeviceLogger currentDeviceLogger)
        {
            return Task.FromResult(Result.Create(true));
        }

        public Action<bool> LastQueryStatusChangedAction { get; set; }

        public Action ConnectionLostAction { get; }

        public bool IsConnected => false;


        public void CloseConnection()
        {
            //
        }

       

        public object Clone()
        {
            return new OfflineConnection();
        }
    }
}
