using System;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
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
        private readonly ISerializerService _serializerService;

        public DefaultDeviceCreator(Func<IDevice> deviceGettingFunc, ILogService logService,
            Func<IDeviceLogger> deviceLoggerGettingFunc, ITypesContainer container, ISerializerService serializerService)
        {
            _deviceGettingFunc = deviceGettingFunc;
            _logService = logService;
            _deviceLoggerGettingFunc = deviceLoggerGettingFunc;
            _container = container;
            _serializerService = serializerService;
        }

        private string _deviceName;
        private IDeviceConnection _availableConnection;

        public string DeviceDescriptionFilePath { get; set; }

        public string DeviceName
        {
            get { return _deviceName; }
            set { _deviceName = value; }
        }

        public DeviceMetaInfo DeviceMetaInfo { get; set; }


        public IConnectionState ConnectionState { get; set; }

        public IDeviceConnection AvailableConnection
        {
            get { return _availableConnection; }
            set { _availableConnection = value; }
        }

        public IDevice Create()
        {
            IDevice newDevice = _serializerService.DeserializeFromFile<IDevice>(DeviceDescriptionFilePath);
            
            newDevice.DeviceLogger = _deviceLoggerGettingFunc();
            _logService.AddLogger(newDevice.DeviceLogger, newDevice.Name);
            return newDevice;
        }

    }
}