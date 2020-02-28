using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class DeviceDataPropertyMemorySubscription : IDeviceDataMemorySubscription
    {
        private readonly IProperty _property;
        private readonly ILocalAndDeviceValueContainingViewModel _localAndDeviceValueContainingViewModel;
        private readonly ushort _offset;
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private readonly IConfigurationMemory _configurationMemory;

        public DeviceDataPropertyMemorySubscription(IProperty property,
            ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,
            IValueViewModelFactory valueViewModelFactory, IConfigurationMemory configurationMemory, ushort offset = 0)
        {
            _property = property;
            _localAndDeviceValueContainingViewModel = localAndDeviceValueContainingViewModel;
            _valueViewModelFactory = valueViewModelFactory;
            _offset = offset;
            _configurationMemory = configurationMemory;
        }

        public void Execute()
        {
            IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();

            var value = formattingService.FormatValue(_property?.UshortsFormatter, MemoryAccessor.GetUshortsFromMemory(
                _configurationMemory,
                (ushort) (_property.Address + _offset), _property.NumberOfPoints, false));
            _localAndDeviceValueContainingViewModel.DeviceValue = _valueViewModelFactory.CreateFormattedValueViewModel(value);
        }
    }
}