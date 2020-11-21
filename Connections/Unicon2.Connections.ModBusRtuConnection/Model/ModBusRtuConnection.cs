using NModbus4;
using NModbus4.Device;
using NModbus4.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Connections.DataProvider.Model;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Model
{

    /// <summary>
    /// класс подключения по ModBusRtu
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
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
            _connectionManager = connectionManager;
            _container = container;
            _localizerService = localizerService;
            _comPortConfigurationFactory = comPortConfigurationFactory;

            ComPortConfiguration = _comPortConfigurationFactory?.CreateComPortConfiguration();
            if (_container == null) 
            {
	            _connectionManager = StaticContainer.Container.Resolve<IComConnectionManager>();
	            _container = StaticContainer.Container.Resolve<ITypesContainer>();
                _localizerService = StaticContainer.Container.Resolve<ILocalizerService>();
                _comPortConfigurationFactory = StaticContainer.Container.Resolve<IComPortConfigurationFactory>();
            }
        }

        [JsonProperty]
        public string PortName { get; set; }

        [JsonProperty]
        public IComPortConfiguration ComPortConfiguration { get; set; }

        public List<string> GetAvailablePorts()
        {
            return _connectionManager.GetSerialPortNames();
        }

      


        /// <summary>
        /// название подключения
        /// </summary>
        public string ConnectionName => "ModBus RTU";

        public async Task<Result> TryOpenConnectionAsync(IDeviceLogger deviceLogger)
        {
            try
            {
                IStreamResource streamResource;
                IModbusSerialMaster modbusSerialMaster = null;
                await Task.Run(() =>
                {
                    streamResource = _connectionManager.GetSerialPortAdapter(PortName);
                    modbusSerialMaster = ModbusSerialMaster.CreateRtu(streamResource);
                });

                if (modbusSerialMaster != null)
                {
                    _currentModbusMaster?.Dispose();
                    _currentModbusMaster = modbusSerialMaster;
                }
                else
                {
                    throw new Exception();
                }
                _slaveId = SlaveId;
                _deviceLogger = deviceLogger;

                _isConnectionLost = false;
                _lastQuerySucceed = true;
            }
            catch (Exception e)
            {
	            return Result.Create(e);
            }
            return Result.Create(true);
        }

        public void CloseConnection()
        {
            _currentModbusMaster?.Dispose();

        }


        [JsonProperty]

        public byte SlaveId
        {
            get { return _slaveId; }
            set { _slaveId = value; }
        }

        protected override void OnDisposing()
        {
            _currentModbusMaster?.Dispose();
            base.OnDisposing();
        }

        public object Clone()
        {
            IModbusRtuConnection modbusRtuConnection = _container.Resolve<IModbusRtuConnection>();
            modbusRtuConnection.ComPortConfiguration = ComPortConfiguration.Clone() as IComPortConfiguration;
            modbusRtuConnection.PortName = PortName;
            modbusRtuConnection.SlaveId = SlaveId;
            return modbusRtuConnection;
        }

        protected override void LogQuery(bool isSuccessful, string dataTitle, string queryDescription, string queryResult = "", Exception exception = null)
        {
            base.LogQuery(isSuccessful, dataTitle, queryDescription, queryResult, exception);
            string localizedDataTitle;
            _localizerService.TryGetLocalizedString(dataTitle, out localizedDataTitle);
            if (isSuccessful)
            {
                if (_isConnectionLost)
                {
                    _isConnectionLost = false;
                    _lastQuerySucceed = true;
                }
                _deviceLogger?.LogSuccessfulQuery("[" + queryDescription + "]" + " " + localizedDataTitle + ". " + queryResult);
            }
            else
            {
                if (!_isConnectionLost)
                {
                    _isConnectionLost = true;
                    _lastQuerySucceed = false;
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
                _deviceLogger?.LogFailedQuery($"[{queryDescription}] {localizedDataTitle}  {queryResult}");
            }
        }
    }

}
