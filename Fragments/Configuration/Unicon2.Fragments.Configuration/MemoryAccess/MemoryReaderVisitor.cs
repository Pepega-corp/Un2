﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class MemoryReaderVisitor : IConfigurationItemVisitor<Task>
    {
        private readonly IDeviceConfiguration _configuration;
        private readonly IDeviceEventsDispatcher _deviceEventsDispatcher;
        private readonly IDeviceMemory _memory;
        public MemoryReaderVisitor(IDeviceConfiguration configuration,
            IDeviceEventsDispatcher deviceEventsDispatcher,
            IDeviceMemory deviceMemory)
        {
            _configuration = configuration;
            _deviceEventsDispatcher = deviceEventsDispatcher;
            _memory = deviceMemory;
        }

        public async Task ExecuteRead()
        {
            await ApplyMemorySettings();
            foreach (var rootConfigurationItem in _configuration.RootConfigurationItemList)
            {
                await rootConfigurationItem.Accept(this);
            }
        }

        private async Task ApplyMemorySettings()
        {
            IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext =
                StaticContainer.Container.Resolve<IQuickAccessMemoryApplyingContext>();

            quickAccessMemoryApplyingContext.OnFillAddressRange =
                (range) =>
                {
                    ushort rangeFrom = (ushort)range.RangeFrom;
                    ushort rangeTo = (ushort)range.RangeTo;
                    return ReadRange(_configuration.DataProvider, rangeFrom, rangeTo, _memory);
                };

            Task applySettingByKey = _configuration.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);

            if (applySettingByKey != null)
                await applySettingByKey;
        }
     

        public async Task VisitItemsGroup(IItemsGroup itemsGroup)
        {
            foreach (var configurationItemInGroup in itemsGroup.ConfigurationItemList)
            {
                await configurationItemInGroup.Accept(this);
            }
        }

        public async Task VisitProperty(IProperty property)
        {
            await ReadRange(_configuration.DataProvider, property.Address,
                (ushort) (property.Address + property.NumberOfPoints), _memory);
        }

        public async Task VisitComplexProperty(IComplexProperty property)
        {
            throw new NotImplementedException();
        }

        public async Task VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task VisitDependentProperty(IDependentProperty dependentPropertyViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task VisitSubProperty(ISubProperty dependentPropertyViewModel)
        {
            throw new NotImplementedException();
        }

        private async Task ReadRange(IDataProvider dataProvider, ushort rangeFrom, ushort rangeTo,
            IDeviceMemory memory)
        {
            if (IfMemoryContainsRange(memory.DeviceMemoryValues, rangeFrom, rangeTo))
            {
                return;
            }

            var res = await dataProvider.ReadHoldingResgistersAsync(rangeFrom,
                (ushort) (rangeTo - rangeFrom), ConfigurationKeys.READING_CONFIGURATION_QUERY);
            if (res.IsSuccessful)
            {
                for (var i = rangeFrom; i < rangeTo; i++)
                {
                    memory.DeviceMemoryValues[i] = res.Result[i - rangeFrom];
                }
            }

            _deviceEventsDispatcher.TriggerDeviceAddressSubscription(rangeFrom,
                (ushort) (rangeTo - rangeFrom));

        }


        private bool IfMemoryContainsRange(Dictionary<ushort, ushort> memorySet, ushort rangeFrom, ushort rangeTo)
        {
            for (var i = rangeFrom; i <= rangeTo; i++)
            {
                if (!memorySet.ContainsKey(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}