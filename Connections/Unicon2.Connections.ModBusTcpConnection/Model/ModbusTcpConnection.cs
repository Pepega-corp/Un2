using NModbus4;
using NModbus4.Device;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Connections.DataProvider.Model;
using Unicon2.Connections.ModBusTcpConnection.Interfaces;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Connections.ModBusTcpConnection.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ModbusTcpConnection : ModbusDataProvider, IModbusTcpConnection
    {
        private readonly IQueryResultFactory _queryResultFactory;
        private ILocalizerService _localizerService;
        private IDeviceLogger _currentDeviceLogger;
        private bool _isConnectionLost;

        private const int PORT_DEFAULT = 502;
        private const string IP_DEFAULT = "192.168.0.1";

        public ModbusTcpConnection(IQueryResultFactory queryResultFactory, ILocalizerService localizerService) : base(
            queryResultFactory)
        {
            _queryResultFactory = queryResultFactory;
            _localizerService = localizerService;

            Port = PORT_DEFAULT;
            IpAddress = IP_DEFAULT;
        }

        protected override void LogQuery(bool isSuccessful, string dataTitle, string queryDescription,
            string queryResult = "",
            Exception exception = null)
        {
            base.LogQuery(isSuccessful, dataTitle, queryDescription, queryResult, exception);
            string localizedDataTitle = dataTitle;
            _localizerService.TryGetLocalizedString(dataTitle, out localizedDataTitle);
            if (isSuccessful)
            {
                if (_isConnectionLost)
                {
                    _isConnectionLost = false;
                    LastQueryStatusChangedAction?.Invoke(true);
                }

                _currentDeviceLogger.LogSuccessfulQuery(
                    "[" + queryDescription + "]" + " " + localizedDataTitle + ". " + queryResult);
            }
            else
            {
                if (!_isConnectionLost)
                {
                    _isConnectionLost = true;
                    LastQueryStatusChangedAction?.Invoke(false);
                }

                string exceptionDescription;
                if (exception is SlaveException)
                {
                    _localizerService.TryGetLocalizedString((exception as SlaveException).MessageKey,
                        out exceptionDescription);
                }
                else
                {
                    exceptionDescription = exception?.Message;
                }

                queryResult = exceptionDescription;
                _currentDeviceLogger.LogFailedQuery($"[{queryDescription}] {localizedDataTitle}  {queryResult}");
            }
        }

        public object Clone()
        {
            return new ModbusTcpConnection(_queryResultFactory, _localizerService)
                {Port = Port, IpAddress = IpAddress};
        }

        public string ConnectionName => "ModBus TCP";

        public async Task<Result> TryOpenConnectionAsync(IDeviceLogger currentDeviceLogger)
        {
            try
            {
                await Task.Run(() =>
                {
                    TcpClient client = new TcpClient(IpAddress, Port);

                    _currentModbusMaster = ModbusIpMaster.CreateIp(client);

                    _currentDeviceLogger = currentDeviceLogger;
                });

                return Result.Create(true);
            }
            catch (Exception e)
            {
                return Result.Create(e);
            }
        }

        public Action<bool> LastQueryStatusChangedAction { get; set; }

        public void CloseConnection()
        {
            _currentModbusMaster.Dispose();
        }

        [JsonProperty] public int Port { get; set; }
        [JsonProperty] public string IpAddress { get; set; }
    }
}