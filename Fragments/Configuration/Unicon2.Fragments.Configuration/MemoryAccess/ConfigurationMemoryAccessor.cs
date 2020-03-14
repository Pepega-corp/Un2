using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class ConfigurationMemoryAccessor
    {
        private readonly IDeviceMemory _memory;
        private readonly IDeviceConfiguration _configuration;
        private readonly IDeviceEventsDispatcher _deviceEventsDispatcher;
        private readonly MemoryAccessEnum _memoryAccessEnum;

        public ConfigurationMemoryAccessor(IDeviceConfiguration configuration,
            IDeviceEventsDispatcher deviceEventsDispatcher, MemoryAccessEnum memoryAccessEnum,
            IDeviceMemory deviceMemory)
        {
            _configuration = configuration;
            _deviceEventsDispatcher = deviceEventsDispatcher;
            _memoryAccessEnum = memoryAccessEnum;
            _memory = deviceMemory;
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
                        (ushort) (property.Address + property.NumberOfPoints + offset), _memory);
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

                    _deviceEventsDispatcher.TriggerLocalAddressSubscription(rangeFrom,
                        (ushort) (rangeTo - rangeFrom));
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
        TransferFromDeviceToLocal
    }
}