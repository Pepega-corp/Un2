using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Services
{
    public class DevicesContainerService : IDevicesContainerService
    {
        private readonly Func<IDevice> _deviceGettingFunc;
        private readonly Func<IDeviceCreator> _deviceCreatorGettingFunc;
        private readonly ILogService _logService;
        private readonly ILocalizerService _localizerService;
        private readonly ISerializerService _serializerService;

        public DevicesContainerService(Func<IDevice> deviceGettingFunc, Func<IDeviceCreator> deviceCreatorGettingFunc,
            ILogService logService, ILocalizerService localizerService, ISerializerService serializerService)
        {
            this._deviceGettingFunc = deviceGettingFunc;
            this._deviceCreatorGettingFunc = deviceCreatorGettingFunc;
            this._logService = logService;
            this._localizerService = localizerService;
            _serializerService = serializerService;
            this.ConnectableItems = new List<IConnectable>();
        }

        public List<IConnectable> ConnectableItems { get; set; }

        public Action<ConnectableItemChangingContext> ConnectableItemChanged { get; set; }

        public List<IDeviceCreator> Creators { get; private set; }


        public void AddConnectableItem(IConnectable device)
        {
            this.ConnectableItems.Add(device);
            this.ConnectableItemChanged?.Invoke(new ConnectableItemChangingContext(device, ItemModifyingTypeEnum.Add));
        }

        public async Task<bool> ConnectDeviceAsync(IDevice device, IDeviceConnection deviceConnection)
        {
            try
            {
                await deviceConnection.TryOpenConnectionAsync(true, device.DeviceLogger);
            }
            catch (Exception e)
            {
                device.Dispose();
                throw e;
            }

            //инициализация подключения (добавление логгеров, датапровайдеров)
            device.InitializeConnection(deviceConnection);
            await device.ConnectionState.CheckConnection();
            //ниже не работает как надо, запихивает девайс в любом случае, даже если переподключаться к одному и тому же устройству
            if (!this.ConnectableItems.Contains(device))
            {
                this.AddConnectableItem(device);
            }

            return true;
        }

        public void RemoveConnectableItem(IConnectable device)
        {
            try
            {
                //this.ConnectableItems.RemoveAt(this.ConnectableItems.FindIndex(o => o.Equals(device)));
                this.ConnectableItems.Remove(device);
            }
            catch (ArgumentNullException _ane)
            {
            }
            catch (ArgumentOutOfRangeException _aofr)
            {
            }
        }

        public void LoadDevicesDefinitions(string folderPath = ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH)
        {
            if (!(Directory.Exists(folderPath))) return;
            string[] names = Directory.GetFiles(folderPath).Select((s => Path.GetFileName(s))).ToArray();
            this.Creators = new List<IDeviceCreator>();

            foreach (string name in names.Where(name => name.Contains("json")))
            {
                IDeviceCreator deviceCreator = this._deviceCreatorGettingFunc();

                deviceCreator.DeviceDescriptionFilePath = Path.Combine(folderPath, name);

                IDevice device = this._deviceGettingFunc();
                try
                {
                    device= _serializerService.DeserializeFromFile<IDevice>(deviceCreator.DeviceDescriptionFilePath);
                    deviceCreator.ConnectionState = device.ConnectionState.Clone() as IConnectionState;
                    device.Dispose();
                    deviceCreator.DeviceName = Path.GetFileNameWithoutExtension(name);
                    this.Creators.Add(deviceCreator);
                }
                catch
                {
                    var message =
                        this._localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages
                            .DEVICE_READING_ERROR);
                    this._logService.LogMessage(message + " " + name);
                }
            }
        }

        public void UpdateDeviceDefinition(string deviceName)
        {
            IDeviceCreator creatorFinded = this.Creators.FirstOrDefault((creator => creator.DeviceName == deviceName));
            if (creatorFinded == null) return;
            IDevice device = this._deviceGettingFunc();
            device=_serializerService.DeserializeFromFile<IDevice>(creatorFinded.DeviceDescriptionFilePath);
            creatorFinded.ConnectionState = device.ConnectionState.Clone() as IConnectionState;
            device.Dispose();
        }

        public void Refresh()
        {
            this.ConnectableItemChanged?.Invoke(
                new ConnectableItemChangingContext(null, ItemModifyingTypeEnum.Refresh));
            foreach (var item in ConnectableItems)
            {
                try
                {
                    item.DeviceConnection.CloseConnection();
                }
                catch (Exception ex)
                {
                }
            }

            this.ConnectableItems.Clear();
        }



    }
}