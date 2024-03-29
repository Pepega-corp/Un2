﻿using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Infrastructure.DeviceContext
{
    public class DeviceContext
    {
        public DeviceContext(IDeviceMemory deviceMemory, IDeviceEventsDispatcher deviceEventsDispatcher,
            string deviceName, IDataProviderContainer dataProviderContainer, IDeviceSharedResources deviceSharedResources)
        {
            DeviceMemory = deviceMemory;
            DeviceEventsDispatcher = deviceEventsDispatcher;
            DeviceName = deviceName;
            DataProviderContainer = dataProviderContainer;
            DeviceSharedResources = deviceSharedResources;
        }

        public IDeviceMemory DeviceMemory { get; }
        public IDeviceEventsDispatcher DeviceEventsDispatcher { get; }
        public string DeviceName { get; }
        public IDataProviderContainer DataProviderContainer { get; }
		public IDeviceSharedResources DeviceSharedResources { get; }
    }

    public class FormattingContext
    {
        public FormattingContext(IFormattedValueOwner valueOwner, DeviceContext deviceContext, bool isLocal)
        {
            ValueOwner = valueOwner;
            DeviceContext = deviceContext;
            IsLocal = isLocal;
        }

        public bool IsLocal { get; }
        public DeviceContext DeviceContext { get; }
        public IFormattedValueOwner ValueOwner { get; }
    }

    public interface IFormattedValueOwner
    {

    }
}