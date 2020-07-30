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
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class MemoryWriterVisitor : IConfigurationItemVisitor<Task>
    {
        private readonly IDeviceConfiguration _configuration;
        private readonly DeviceContext _deviceContext;
        private List<ushort> _writtenAddresses;
        private int _offset;

        public MemoryWriterVisitor(DeviceContext deviceContext, List<ushort> writtenAddresses,
            IDeviceConfiguration configuration, int offset)
        {
            _configuration = configuration;
            _offset = offset;
            _deviceContext = deviceContext;
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
                    return WriteRange(_deviceContext.DataProviderContainer.DataProvider, rangeFrom, rangeTo, _deviceContext.DeviceMemory);
                };

            Task applySettingByKey = _configuration.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);

            if (applySettingByKey != null)
                await applySettingByKey;
            
            IActivatedSettingApplyingContext activatedApplyingContext =
                StaticContainer.Container.Resolve<IActivatedSettingApplyingContext>();
            activatedApplyingContext.DataProvider = _deviceContext.DataProviderContainer.DataProvider;
            Task applyActivationSettingByKey = _configuration.FragmentSettings?.ApplySettingByKey(
                ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING,
                activatedApplyingContext);

            if (applyActivationSettingByKey != null)
                await applyActivationSettingByKey;
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
				        await configurationItemInGroup.Accept(new MemoryWriterVisitor(_deviceContext, _writtenAddresses,
					        _configuration, offset));
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
	        await WriteRange(_deviceContext.DataProviderContainer.DataProvider, (ushort) (property.Address + _offset),
		        (ushort) (property.Address + _offset + property.NumberOfPoints), _deviceContext.DeviceMemory);
        }

        public async Task VisitComplexProperty(IComplexProperty property)
        {
	        await VisitProperty(property);
		}

        public Task VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task VisitDependentProperty(IDependentProperty dependentProperty)
        {
			await VisitProperty(dependentProperty);
		}

        public async Task VisitSubProperty(ISubProperty subProperty)
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

                _deviceContext.DeviceEventsDispatcher.TriggerDeviceAddressSubscription(rangeFrom,
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