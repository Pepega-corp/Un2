﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services.ItemChangingContext;

namespace Unicon2.Infrastructure.Services
{
    public interface IDevicesContainerService
    {
        List<IDeviceCreator> Creators { get; }
        List<IConnectable> ConnectableItems { get; set; }

        Action<ConnectableItemChangingContext> ConnectableItemChanged { get; set; }

        void AddConnectableItem(IConnectable device);
        void RemoveConnectableItem(IConnectable device);

        Task<Result> ConnectDeviceAsync(IDevice device, IDeviceConnection deviceConnection);

        
        void LoadDevicesDefinitions(string folderPath="Devices");

        void UpdateDeviceDefinition(string deviceName);
        void DeleteDeviceDefinition(string deviceName, string folderPath = "Devices");

        void Refresh();
    }
}