using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Factories
{
    public class BaseValuesViewModelFactory
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ILocalizerService _localizerService;

        public BaseValuesViewModelFactory(IApplicationGlobalCommands applicationGlobalCommands,
            ILocalizerService localizerService)
        {
            _applicationGlobalCommands = applicationGlobalCommands;
            _localizerService = localizerService;
        }

        public IRuntimeBaseValuesViewModel CreateRuntimeBaseValuesViewModel(
            IConfigurationBaseValues configurationBaseValues, DeviceContext deviceContext)
        {
            if (configurationBaseValues == null)
            {
                return new RuntimeBaseValuesViewModel(new List<IRuntimeBaseValueViewModel>());
            }

            return new RuntimeBaseValuesViewModel(configurationBaseValues.BaseValues.Select(value =>
                new RuntimeBaseValueViewModel(value.Name, new RelayCommand(
                    () =>
                    {
                        if (_applicationGlobalCommands.AskUserGlobal(
                            _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings
                                .ARE_YOU_SURE_TO_APPLY_VALUES) + ": " + value.Name,
                            _localizerService.GetLocalizedString("BasicValues")))
                        {
                            var addresses = value.LocalMemoryValues.Keys.ToArray();
                            foreach (var address in addresses)
                            {
                                if (deviceContext.DeviceMemory.LocalMemoryValues.ContainsKey(address))
                                {
                                    deviceContext.DeviceMemory.LocalMemoryValues[address] =
                                        value.LocalMemoryValues[address];
                                }
                                else
                                {
                                    deviceContext.DeviceMemory.LocalMemoryValues.Add(address,
                                        value.LocalMemoryValues[address]);
                                }
                            }

                            foreach (var address in addresses)
                            {
                                deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
                                    address, 1, MemoryKind.UshortMemory);
                            }
                        }
                    }
                ))).Cast<IRuntimeBaseValueViewModel>().ToList());
        }
    }
}