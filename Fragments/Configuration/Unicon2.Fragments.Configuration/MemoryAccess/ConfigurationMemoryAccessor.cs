﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Unicon2.Fragments.Configuration.Helpers;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Model;
using Unicon2.Fragments.Configuration.Model.Memory;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class ConfigurationMemoryAccessor
    {
        private readonly IConfigurationMemory _memory;
        private readonly IDeviceConfiguration _configuration;
        private readonly IDeviceEventsDispatcher _deviceEventsDispatcher;
        private readonly MemoryAccessEnum _memoryAccessEnum;

        public ConfigurationMemoryAccessor(IDeviceConfiguration configuration, IDeviceEventsDispatcher deviceEventsDispatcher, MemoryAccessEnum memoryAccessEnum)
        {
            _configuration = configuration;
            _deviceEventsDispatcher = deviceEventsDispatcher;
            _memoryAccessEnum = memoryAccessEnum;
            if (configuration.ConfigurationMemory == null)
            {
                _memory = new ConfigurationMemory();
                configuration.ConfigurationMemory = _memory;
            }
            else
                _memory = configuration.ConfigurationMemory;
        }

        public async Task Process()
        {
            if (_memoryAccessEnum == MemoryAccessEnum.Read)
            {
                _memory.DeviceMemoryValues.Clear();
            }
            await ApplyMemorySettings();
            await ProcessProperties();
        }


        private async Task ProcessProperties()
        {
            foreach (var rootConfigurationItem in _configuration.RootConfigurationItemList)
            {
                await ProcessConfigurationItem(rootConfigurationItem, 0);
            }
        }

        private async Task ProcessConfigurationItem(IConfigurationItem configurationItem, ushort offset)
        {
            switch (configurationItem)
            {
                case IItemsGroup itemsGroup:
                    if (itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo &&
                        groupWithReiterationInfo.IsReiterationEnabled)
                    {
                        await ProcessGroupWithreiteration(itemsGroup, groupWithReiterationInfo, offset);
                    }
                    else
                    {
                        foreach (var configurationItemInGroup in itemsGroup.ConfigurationItemList)
                        {
                            await ProcessConfigurationItem(configurationItemInGroup, offset);
                        }
                    }
                    break;
                case IComplexProperty complexProperty:
                    await ProcessComplexProperty(complexProperty, offset);
                    break;
                case IProperty property:
                    await ProcessAddressRange(_configuration.DataProvider, property.Address,
                        (ushort) (property.Address + offset + property.NumberOfPoints + offset), _memory);
                    break;
            }
        }

        private async Task ProcessGroupWithreiteration(IItemsGroup itemsGroup,
            IGroupWithReiterationInfo groupWithReiterationInfo, ushort offsetInitial)
        {
            var offset = offsetInitial;
            foreach (var _ in groupWithReiterationInfo.SubGroups)
            {
                foreach (var configurationItem in itemsGroup.ConfigurationItemList)
                {
                    await ProcessConfigurationItem(configurationItem, offset);
                }

                offset = (ushort) (offset + groupWithReiterationInfo.ReiterationStep);
            }
        }

        private async Task ProcessComplexProperty(IComplexProperty complexProperty, ushort offset)
        {
            foreach (var configurationItemInGroup in complexProperty.SubProperties)
            {
                await ProcessConfigurationItem(configurationItemInGroup, offset);
            }
        }

        private async Task ApplyMemorySettings()
        {
            IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext =
                StaticContainer.Container.Resolve<IQuickAccessMemoryApplyingContext>();


            quickAccessMemoryApplyingContext.OnFillAddressRange =
                (range) =>
                {
                    ushort rangeFrom = (ushort) range.RangeFrom;
                    ushort rangeTo = (ushort) range.RangeTo;
                    return ProcessAddressRange(_configuration.DataProvider, rangeFrom, rangeTo, _memory);
                };


            Task applySettingByKey = _configuration.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);

            if (applySettingByKey != null)
                await applySettingByKey;
        }



        private async Task ProcessAddressRange(IDataProvider dataProvider, ushort rangeFrom, ushort rangeTo,
            IConfigurationMemory memory)
        {
            switch (_memoryAccessEnum)
            {
                case MemoryAccessEnum.Read:
                {
                    if (IfMemoryContainsRange(memory.DeviceMemoryValues, rangeFrom, rangeTo))
                    {
                        return;
                    }

                    var res = await dataProvider.ReadHoldingResgistersAsync(rangeFrom,
                        (ushort) (rangeTo - rangeFrom + 1), ConfigurationKeys.READING_CONFIGURATION_QUERY);
                    if (res.IsSuccessful)
                    {
                        for (var i = rangeFrom; i <= rangeTo; i++)
                        {
                            memory.DeviceMemoryValues[i] = res.Result[i - rangeFrom];
                        }
                    }

                    _memoryBusDispatcher.TriggerDeviceDataSubscriptionByAddress(rangeFrom,
                        (ushort) (rangeTo - rangeFrom));
                    break;
                }
                case MemoryAccessEnum.Write:
                {
                    var valuesToWrite = new List<ushort>();
                    for (var i = rangeFrom; i < rangeTo; i++)
                    {
                        valuesToWrite.Add(memory.LocalMemoryValues[i]);
                    }

                    var res = await dataProvider.WriteMultipleRegistersAsync(rangeFrom,
                        valuesToWrite.ToArray(), ConfigurationKeys.WRITING_CONFIGURATION_QUERY);
                    break;
                }
                case MemoryAccessEnum.InitializeLocalValues:
                    for (var i = rangeFrom; i <= rangeTo; i++)
                    {
                        memory.DeviceMemoryValues[i] = 0;
                    }

                    break;
                case MemoryAccessEnum.TransferFromLocalToDevice:
                    for (var i = rangeFrom; i <= rangeTo; i++)
                    {
                        if (memory.LocalMemoryValues.ContainsKey(i))
                        {
                            memory.DeviceMemoryValues[i] = memory.LocalMemoryValues[i];
                        }
                        else
                        {
                            memory.DeviceMemoryValues[i] = 0;
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

    public enum MemoryAccessEnum
    {
        Read,
        Write,
        InitializeLocalValues,
        TransferFromLocalToDevice,

    }
}