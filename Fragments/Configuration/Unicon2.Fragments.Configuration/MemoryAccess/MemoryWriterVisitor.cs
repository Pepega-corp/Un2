using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class MemoryWriterVisitor : IConfigurationItemVisitor<Task>
    {
        private readonly IDeviceConfiguration _configuration;
        private readonly IDeviceEventsDispatcher _deviceEventsDispatcher;
        private readonly IDeviceMemory _memory;
        private List<ushort> _writtenAddresses;
        public MemoryWriterVisitor(IDeviceConfiguration configuration,
            IDeviceEventsDispatcher deviceEventsDispatcher,
            IDeviceMemory deviceMemory, List<ushort> writtenAddresses)
        {
            _configuration = configuration;
            _deviceEventsDispatcher = deviceEventsDispatcher;
            _memory = deviceMemory;
            _writtenAddresses = writtenAddresses;
        }

        public async Task ExecuteWrite()
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
                    return WriteRange(_configuration.DataProvider, rangeFrom, rangeTo, _memory);
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
            await WriteRange(_configuration.DataProvider, property.Address,
                (ushort)(property.Address + property.NumberOfPoints), _memory);
        }

        public Task VisitComplexProperty(IComplexProperty property)
        {
            throw new System.NotImplementedException();
        }

        public Task VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task VisitDependentProperty(IDependentProperty dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task VisitSubProperty(ISubProperty dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }

        private async Task WriteRange(IDataProvider dataProvider, ushort rangeFrom, ushort rangeTo,
            IDeviceMemory memory)
        {
            if (IfWritten(_writtenAddresses, rangeFrom, rangeTo))
            {
                return;
            }
            var valuesToWrite = new List<ushort>();

            for (var i = rangeFrom; i < rangeTo; i++)
            {
                // если значения пришли из пачки и не использовались
                if (memory.LocalMemoryValues.ContainsKey(i))
                {
                    valuesToWrite.Add(memory.LocalMemoryValues[i]);
                }
                else if(memory.DeviceMemoryValues.ContainsKey(i))
                {
                    valuesToWrite.Add(memory.DeviceMemoryValues[i]);
                }
                else
                {
                    return;
                }
            }

            var res = await dataProvider.WriteMultipleRegistersAsync(rangeFrom,
                valuesToWrite.ToArray(), ConfigurationKeys.WRITING_CONFIGURATION_QUERY);
            if (res.IsSuccessful)
            {
                for (var i = rangeFrom; i < rangeTo; i++)
                {
                    if (memory.LocalMemoryValues.ContainsKey(i))
                    {
                        memory.DeviceMemoryValues[i] = memory.LocalMemoryValues[i];
                    }
                    else
                    {
                        memory.DeviceMemoryValues[i] = 0;
                    }
                    _writtenAddresses.Add(i);
                }

                _deviceEventsDispatcher.TriggerDeviceAddressSubscription(rangeFrom,
                    (ushort) (rangeTo - rangeFrom));
            }
        }

        private bool IfWritten(List<ushort> writtenAddresses, ushort rangeFrom, ushort rangeTo)
        {
            for (int i = rangeFrom; i < rangeTo; i++)
            {
                if (writtenAddresses.All(arg => arg != i))
                {
                    return false;
                }
            }
            return true;
        }
    }
}