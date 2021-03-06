﻿using System;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class ConfigurationMemoryAccessor
    {
        private readonly MemoryAccessEnum _memoryAccessEnum;
        private readonly bool _triggerSubscriptions;
        private readonly IDeviceConfiguration _configuration;
        private readonly DeviceContext _deviceContext;

        public ConfigurationMemoryAccessor(IDeviceConfiguration configuration, DeviceContext deviceContext,
            MemoryAccessEnum memoryAccessEnum,bool triggerSubscriptions)
        {
            _configuration = configuration;
            _deviceContext = deviceContext;
            _memoryAccessEnum = memoryAccessEnum;
            _triggerSubscriptions = triggerSubscriptions;
        }

        public async Task Process()
        {
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
                    await ProcessAddressRange(_deviceContext.DataProviderContainer.DataProvider.Item,
                        (ushort) (complexProperty.Address + offset),
                        (ushort) (complexProperty.Address + complexProperty.NumberOfPoints + offset),
                        _deviceContext.DeviceMemory);
                    break;
                case IProperty property:
                    await ProcessAddressRange(_deviceContext.DataProviderContainer.DataProvider.Item,
                        (ushort) (property.Address + offset),
                        (ushort) (property.Address + property.NumberOfPoints + offset), _deviceContext.DeviceMemory);
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



        private async Task ApplyMemorySettings()
        {
            IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext =
                StaticContainer.Container.Resolve<IQuickAccessMemoryApplyingContext>();


            quickAccessMemoryApplyingContext.OnFillAddressRange =
                (range) =>
                {
                    ushort rangeFrom = (ushort) range.RangeFrom;
                    ushort rangeTo = (ushort) range.RangeTo;
                    return ProcessAddressRange(_deviceContext.DataProviderContainer.DataProvider.Item, rangeFrom,
                        rangeTo,
                        _deviceContext.DeviceMemory);
                };


            Task applySettingByKey = _configuration.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);

            if (applySettingByKey != null)
                await applySettingByKey;
        }



        private Task ProcessAddressRange(IDataProvider dataProvider, ushort rangeFrom, ushort rangeTo,
            IDeviceMemory memory)
        {
            switch (_memoryAccessEnum)
            {

                case MemoryAccessEnum.TransferFromDeviceToLocal:
                    for (var i = rangeFrom; i < rangeTo; i++)
                    {
                        if (memory.DeviceMemoryValues.ContainsKey(i))
                        {
                            memory.LocalMemoryValues[i] = memory.DeviceMemoryValues[i];
                        }
                        else
                        {
                            memory.LocalMemoryValues[i] = 0;
                        }
                    }

                    _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(rangeFrom,
                        (ushort) (rangeTo - rangeFrom));
                    break;
                case MemoryAccessEnum.InitalizeZeroForLocals:
                    for (var i = rangeFrom; i < rangeTo; i++)
                    {
                        if (!memory.LocalMemoryValues.ContainsKey(i))
                        {
                            memory.LocalMemoryValues.Add(i,0);
                        }
                    }

                    if (_triggerSubscriptions)
                    {
                        _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(rangeFrom,
                            (ushort) (rangeTo - rangeFrom));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }

    }

    public enum MemoryAccessEnum
    {
        TransferFromDeviceToLocal,
        InitalizeZeroForLocals
    }
}