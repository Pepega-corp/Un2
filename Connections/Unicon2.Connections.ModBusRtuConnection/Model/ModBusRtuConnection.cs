using NModbus4;
using NModbus4.Device;
using NModbus4.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Connections.DataProvider.Model;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Model
{

    /// <summary>
    /// класс подключения по ModBusRtu
    /// </summary>
    [DataContract(Namespace = "ModBusRtuConnectionNS", IsReference = true)]
    public class ModBusRtuConnection : ModbusDataProvider, IModbusRtuConnection
    {
        private IComConnectionManager _connectionManager;
        private ITypesContainer _container;
        private ILocalizerService _localizerService;
        private IComPortConfigurationFactory _comPortConfigurationFactory;
        private bool _isConnectionLost;
        private IDeviceLogger _deviceLogger;

        public ModBusRtuConnection(IComConnectionManager connectionManager, ITypesContainer container, ILocalizerService localizerService,
            IComPortConfigurationFactory comPortConfigurationFactory, IQueryResultFactory queryResultFactory) : base(queryResultFactory)
        {
            this._connectionManager = connectionManager;
            this._container = container;
            this._localizerService = localizerService;
            this._comPortConfigurationFactory = comPortConfigurationFactory;

            this.ComPortConfiguration = this._comPortConfigurationFactory.CreateComPortConfiguration();
        }

        [DataMember]
        public string PortName { get; set; }

        [DataMember]
        public IComPortConfiguration ComPortConfiguration { get; set; }

        public List<string> GetAvailablePorts()
        {
            return this._connectionManager.GetSerialPortNames();
        }


        /// <summary>
        /// название подключения
        /// </summary>
        public string ConnectionName => "ModBus RTU";

        public async Task<bool> TryOpenConnectionAsync(bool isThrowingException, IDeviceLogger deviceLogger)
        {
            try
            {
                IStreamResource streamResource;
                IModbusSerialMaster modbusSerialMaster = null;
                await Task.Run(() =>
                {
                    streamResource = this._connectionManager.GetSerialPortAdapter(this.PortName);
                    modbusSerialMaster = ModbusSerialMaster.CreateRtu(streamResource);
                });

                if (modbusSerialMaster != null)
                {
                    this._currentModbusMaster?.Dispose();
                    this._currentModbusMaster = modbusSerialMaster;
                }
                else
                {
                    throw new Exception();
                }
                this._slaveId = this.SlaveId;
                this._deviceLogger = deviceLogger;

                this._isConnectionLost = false;
                this._lastQuerySucceed = true;
            }
            catch (Exception)
            {
                if (isThrowingException) throw;
                return false;
            }
            return true;
        }

        public Action<bool> LastQueryStatusChangedAction { get; set; }


        public void CloseConnection()
        {
            this._currentModbusMaster.Dispose();

        }


        [DataMember]

        public byte SlaveId
        {
            get { return this._slaveId; }
            set { this._slaveId = value; }
        }

        protected override void OnDisposing()
        {
            this._currentModbusMaster?.Dispose();
            base.OnDisposing();
        }

        public object Clone()
        {
            IModbusRtuConnection modbusRtuConnection = this._container.Resolve<IModbusRtuConnection>();
            modbusRtuConnection.ComPortConfiguration = this.ComPortConfiguration.Clone() as IComPortConfiguration;
            modbusRtuConnection.PortName = this.PortName;
            modbusRtuConnection.SlaveId = this.SlaveId;
            return modbusRtuConnection;
        }

        protected override void LogQuery(bool isSuccessful, string dataTitle, string queryDescription, string queryResult = "", Exception exception = null)
        {
            base.LogQuery(isSuccessful, dataTitle, queryDescription, queryResult, exception);
            string localizedDataTitle;
            this._localizerService.TryGetLocalizedString(dataTitle, out localizedDataTitle);
            if (isSuccessful)
            {
                if (this._isConnectionLost)
                {
                    this._isConnectionLost = false;
                    this.LastQueryStatusChangedAction?.Invoke(true);
                }
                this._deviceLogger?.LogSuccessfulQuery("[" + queryDescription + "]" + " " + localizedDataTitle + ". " + queryResult);
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
                this._deviceLogger?.LogFailedQuery($"[{queryDescription}] {localizedDataTitle}  {queryResult}");
            }
        }
    }

}
