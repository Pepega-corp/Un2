using NModbus4;
using NModbus4.Device;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Connections.DataProvider.Model;
using Unicon2.Connections.ModBusTcpConnection.Interfaces;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusTcpConnection.Model
{
    [DataContract(Namespace = "ModbusTcpConnectionNS")]
    public class ModbusTcpConnection : ModbusDataProvider, IModbusTcpConnection
    {
        private readonly IQueryResultFactory _queryResultFactory;
        private ILocalizerService _localizerService;
        private IDeviceLogger _currentDeviceLogger;
        private bool _isConnectionLost;

        private const int PORT_DEFAULT = 502;
        private const string IP_DEFAULT = "192.168.0.1";

        public ModbusTcpConnection(IQueryResultFactory queryResultFactory, ILocalizerService localizerService) : base(queryResultFactory)
        {
            this._queryResultFactory = queryResultFactory;
            this._localizerService = localizerService;

            this.Port = PORT_DEFAULT;
            this.IpAddress = IP_DEFAULT;
        }

        protected override void LogQuery(bool isSuccessful, string dataTitle, string queryDescription, string queryResult = "",
            Exception exception = null)
        {
            base.LogQuery(isSuccessful, dataTitle, queryDescription, queryResult, exception);
            string localizedDataTitle = dataTitle;
            this._localizerService.TryGetLocalizedString(dataTitle, out localizedDataTitle);
            if (isSuccessful)
            {
                if (this._isConnectionLost)
                {
                    this._isConnectionLost = false;
                    this.LastQueryStatusChangedAction?.Invoke(true);
                }

                this._currentDeviceLogger.LogSuccessfulQuery("[" + queryDescription + "]" + " " + localizedDataTitle + ". " + queryResult);
            }
            else
            {
                if (!this._isConnectionLost)
                {
                    this._isConnectionLost = true;
                    this.LastQueryStatusChangedAction?.Invoke(false);
                }
                string exceptionDescription;
                if (exception is SlaveException)
                {
                    this._localizerService.TryGetLocalizedString((exception as SlaveException).MessageKey,
                        out exceptionDescription);
                }
                else
                {
                    exceptionDescription = exception?.Message;
                }
                queryResult = exceptionDescription;
                this._currentDeviceLogger.LogFailedQuery($"[{queryDescription}] {localizedDataTitle}  {queryResult}");
            }
        }

        public object Clone()
        {
            return new ModbusTcpConnection(this._queryResultFactory, this._localizerService) { Port = this.Port, IpAddress = this.IpAddress };
        }

        public string ConnectionName => "ModBus TCP";
        public async Task<bool> TryOpenConnectionAsync(bool isThrowingException, IDeviceLogger currentDeviceLogger)
        {
            try
            {
                await Task.Run(() =>
                {
                    TcpClient client = new TcpClient(this.IpAddress, this.Port);

                    this._currentModbusMaster = ModbusIpMaster.CreateIp(client);

                    this._currentDeviceLogger = currentDeviceLogger;
                });

                return true;
            }
            catch (Exception e)
            {

                if (isThrowingException)
                {
                    throw;
                }
                return false;
            }

        }

        public Action<bool> LastQueryStatusChangedAction { get; set; }

        public void CloseConnection()
        {
            this._currentModbusMaster.Dispose();
        }

        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public string IpAddress { get; set; }


        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._localizerService = container.Resolve<ILocalizerService>();
            base.InitializeFromContainer(container);
        }
    }
}
