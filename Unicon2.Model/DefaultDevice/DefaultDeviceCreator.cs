using System;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.DefaultDevice
{
    public class DefaultDeviceCreator : IDeviceCreator
    {
        private readonly Func<IDevice> _deviceGettingFunc;
        private readonly ILogService _logService;
        private readonly Func<IDeviceLogger> _deviceLoggerGettingFunc;
        private readonly ITypesContainer _container;

        public DefaultDeviceCreator(Func<IDevice> deviceGettingFunc, ILogService logService, Func<IDeviceLogger> deviceLoggerGettingFunc, ITypesContainer container)
        {
            this._deviceGettingFunc = deviceGettingFunc;
            this._logService = logService;
            this._deviceLoggerGettingFunc = deviceLoggerGettingFunc;
            this._container = container;
        }

        private string _deviceName;
        private IDeviceConnection _availableConnection;

        public string DeviceDescriptionFilePath { get; set; }

        public string DeviceName
        {
            get { return this._deviceName; }
            set { this._deviceName = value; }
        }

        public IConnectionState ConnectionState { get; set; }

        public IDeviceConnection AvailableConnection
        {
            get { return this._availableConnection; }
            set { this._availableConnection = value; }
        }

        public IDevice Create()
        {
            IDevice newDevice = this._deviceGettingFunc();
            newDevice.DeserializeFromFile(this.DeviceDescriptionFilePath);
            newDevice.InitializeFromContainer(this._container);
            newDevice.Name = this.DeviceName;
            newDevice.DeviceSignature = this.DeviceName;
            newDevice.DeviceLogger = this._deviceLoggerGettingFunc();
            this._logService.AddLogger(newDevice.DeviceLogger, newDevice.Name);
            return newDevice;
        }

    }
}