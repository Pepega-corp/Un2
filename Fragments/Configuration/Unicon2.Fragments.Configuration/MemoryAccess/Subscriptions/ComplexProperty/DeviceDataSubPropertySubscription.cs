using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
    public class DeviceDataSubPropertySubscription : IDeviceDataMemorySubscription
    {
        private readonly ISubProperty _subProperty;
        private readonly IRuntimeSubPropertyViewModel _runtimeSubPropertyViewModel;
        private readonly ushort _offset;
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private DeviceContext _deviceContext;

        public int Priority { get; set; } = 1;

        public DeviceDataSubPropertySubscription(ISubProperty subProperty,
            IRuntimeSubPropertyViewModel runtimeSubPropertyViewModel, ushort offset,
            IValueViewModelFactory valueViewModelFactory, DeviceContext deviceContext)
        {
            _subProperty = subProperty;
            _runtimeSubPropertyViewModel = runtimeSubPropertyViewModel;
            _offset = offset;
            _valueViewModelFactory = valueViewModelFactory;
            _deviceContext = deviceContext;
        }



        public void Execute()
        {

            IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();

            var formatterForProperty = _subProperty?.UshortsFormatter;
            if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                (ushort) (_subProperty.Address + _offset), _subProperty.NumberOfPoints, false))
            {
                return;
            }
            var ushortsFromDevice = MemoryAccessor.GetUshortsFromMemory(
                _deviceContext.DeviceMemory,
                (ushort) (_subProperty.Address + _offset), _subProperty.NumberOfPoints, false);

            var boolArray = ushortsFromDevice.GetBoolArrayFromUshortArray().ToArray();
            List<bool> subPropertyBools = new List<bool>();
            foreach (var bitNumber in _subProperty.BitNumbersInWord)
            {
                subPropertyBools.Add(boolArray[bitNumber]);
            }

            subPropertyBools.Reverse();
            var subPropertyUshort = subPropertyBools.BoolArrayToUshort();
            if (_subProperty?.Dependencies?.Count > 0)
            {
                formatterForProperty = DependentSubscriptionHelpers.GetFormatterConsideringDependencies(
                    _subProperty.Dependencies, _deviceContext, formattingService,
                    _subProperty?.UshortsFormatter, _offset);
            }

            var subPropertyValue =
                formattingService.FormatValue(_subProperty.UshortsFormatter, new[] {subPropertyUshort});

            _runtimeSubPropertyViewModel.DeviceValue =
                _valueViewModelFactory.CreateFormattedValueViewModel(subPropertyValue);
        }
    }
}