using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class DeviceDataPropertyMemorySubscription : IDeviceDataMemorySubscription
    {
        private readonly IProperty _property;
        private readonly ILocalAndDeviceValueContainingViewModel _localAndDeviceValueContainingViewModel;
        private readonly ushort _offset;
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private readonly DeviceContext _deviceContext;
        public int Priority { get; set; } = 1;

        public DeviceDataPropertyMemorySubscription(IProperty property,
            ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,
            IValueViewModelFactory valueViewModelFactory, DeviceContext deviceContext, ushort offset)
        {
            _property = property;
            _localAndDeviceValueContainingViewModel = localAndDeviceValueContainingViewModel;
            _valueViewModelFactory = valueViewModelFactory;
            _deviceContext = deviceContext;
            _offset = offset;
        }

        public async void Execute()
        {
            if (_property.IsFromBits)
            {
                IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();

                var formatterForProperty = _property?.UshortsFormatter;
                if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                    (ushort) (_property.Address + _offset), _property.NumberOfPoints, false))
                {
                    return;
                }

                var ushortsFromDevice = MemoryAccessor.GetUshortsFromMemory(
                    _deviceContext.DeviceMemory,
                    (ushort) (_property.Address + _offset), _property.NumberOfPoints, false);

                var boolArray = ushortsFromDevice.GetBoolArrayFromUshortArray().ToArray();
                List<bool> subPropertyBools = new List<bool>();
                foreach (var bitNumber in _property.BitNumbers)
                {
                    subPropertyBools.Add(boolArray[bitNumber]);
                }

                subPropertyBools.Reverse();
                var subPropertyUshort = subPropertyBools.BoolArrayToUshort();
                if (_property?.Dependencies?.Count > 0)
                {
                    formatterForProperty = await DependentSubscriptionHelpers.GetFormatterConsideringDependencies(
                        _property.Dependencies, _deviceContext, formattingService,
                        _property?.UshortsFormatter, _offset);
                }

                var value = await formattingService.FormatValueAsync(formatterForProperty, subPropertyUshort.AsCollection(),new FormattingContext(_localAndDeviceValueContainingViewModel,_deviceContext,false));
                _localAndDeviceValueContainingViewModel.DeviceValue =
                    _valueViewModelFactory.CreateFormattedValueViewModel(value);
            }
            else
            {

                IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();

                var formatterForProperty = _property?.UshortsFormatter;
                if (_property?.Dependencies?.Count > 0)
                {
                    formatterForProperty = await DependentSubscriptionHelpers.GetFormatterConsideringDependencies(
                        _property.Dependencies, _deviceContext, formattingService,
                        _property?.UshortsFormatter, _offset);
                }

                if (MemoryAccessor.IsMemoryContainsAddresses(
                    _deviceContext.DeviceMemory,
                    (ushort) (_property.Address + _offset), _property.NumberOfPoints, false))
                {
                    var value = await formattingService.FormatValueAsync(formatterForProperty,
                        MemoryAccessor.GetUshortsFromMemory(
                            _deviceContext.DeviceMemory,
                            (ushort) (_property.Address + _offset), _property.NumberOfPoints, false), new FormattingContext(_localAndDeviceValueContainingViewModel, _deviceContext, false));
                    _localAndDeviceValueContainingViewModel.DeviceValue =
                        _valueViewModelFactory.CreateFormattedValueViewModel(value);
                }
            }
        }
    }
}