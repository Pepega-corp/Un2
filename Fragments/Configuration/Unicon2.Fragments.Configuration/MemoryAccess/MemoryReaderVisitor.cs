using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{



    public class MemoryReaderVisitor : IConfigurationItemVisitor<Task>
    {
        private readonly IDeviceConfiguration _configuration;
        private readonly DeviceContext _deviceContext;
        private readonly int _offset;
        private readonly bool _triggerSubscriptions;


        public MemoryReaderVisitor(IDeviceConfiguration configuration, DeviceContext deviceContext, int offset,
            bool triggerSubscriptions = true)
        {
            _configuration = configuration;
            _deviceContext = deviceContext;
            _offset = offset;
            _triggerSubscriptions = triggerSubscriptions;
        }

        public async Task ExecuteRead()
        {
            foreach (var rootConfigurationItem in _configuration.RootConfigurationItemList)
            {
                rootConfigurationItem.Accept(new ClearMemoryVisitor(_deviceContext.DeviceMemory.DeviceMemoryValues));
            }

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
                    ushort rangeFrom = (ushort) range.RangeFrom;
                    ushort rangeTo = (ushort) range.RangeTo;
                    MemoryAccessor.ClearRangeTo(rangeFrom, rangeTo, _deviceContext.DeviceMemory.DeviceMemoryValues);
                    return ReadRange(_deviceContext.DataProviderContainer.DataProvider.Item, rangeFrom, rangeTo,
                        _deviceContext.DeviceMemory);
                };

            Task applySettingByKey = _configuration.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);

            if (applySettingByKey != null)
                await applySettingByKey;
        }

     
        public async Task VisitItemsGroup(IItemsGroup itemsGroup)
        {
	        if (itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo &&
	            groupWithReiterationInfo.IsReiterationEnabled)
	        {
		        int offset = _offset;
		        foreach (var subGroup in groupWithReiterationInfo.SubGroups)
		        {
			        foreach (var configurationItemInGroup in itemsGroup.ConfigurationItemList)
			        {
				        await configurationItemInGroup.Accept(new MemoryReaderVisitor(_configuration,_deviceContext, offset,_triggerSubscriptions));
			        }
			        offset += groupWithReiterationInfo.ReiterationStep;
		        }
	        }
	        else
	        {
		        foreach (var configurationItemInGroup in itemsGroup.ConfigurationItemList)
		        {
			        await configurationItemInGroup.Accept(this);
		        }
	        }
        }

        public async Task VisitProperty(IProperty property)
        {

            await ReadRange(_deviceContext.DataProviderContainer.DataProvider.Item, (ushort) (property.Address + _offset),
		        (ushort) (property.Address
		                  + _offset + property.NumberOfPoints), _deviceContext.DeviceMemory);
   
        }

        public async Task VisitComplexProperty(IComplexProperty property)
        {
	        await VisitProperty(property);
        }

        public Task VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new NotImplementedException();
        }

        public Task VisitSubProperty(ISubProperty subProperty)
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
                    if (!memory.LocalMemoryValues.ContainsKey(i))
                    {
                        memory.LocalMemoryValues[i] = res.Result[i - rangeFrom];
                        if (_triggerSubscriptions)
                        {
                            _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(rangeFrom,
                                1);
                        }
                    }


                }

                if (_triggerSubscriptions)
                {
                    _deviceContext.DeviceEventsDispatcher.TriggerDeviceAddressSubscription(rangeFrom,
                        (ushort) (rangeTo - rangeFrom));
                }

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
}
