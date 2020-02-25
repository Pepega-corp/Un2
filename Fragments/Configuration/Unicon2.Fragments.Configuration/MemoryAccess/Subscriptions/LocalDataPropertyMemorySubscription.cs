using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class LocalDataPropertyMemorySubscription : ILocalDataMemorySubscription
    {
        private readonly IConfigurationMemory _configurationMemory;
        private readonly IProperty _property;

        public LocalDataPropertyMemorySubscription(IEditableValueViewModel<IFormattedValue> editableValueViewModel,
            IConfigurationMemory configurationMemory, IProperty property)
        {
            _configurationMemory = configurationMemory;
            _property = property;
            EditableValueViewModel = editableValueViewModel;
        }

        public void Execute()
        {

            var value = _property?.UshortsFormatter.Format(MemoryAccessor.GetUshortsFromMemory(
                _configurationMemory,
                _property.Address, _property.NumberOfPoints, true));

            var memoryValue =
                _property?.UshortsFormatter.FormatBack(EditableValueViewModel.GetValue());
            MemoryAccessor.GetUshortsInMemory(_configurationMemory, _property.Address, memoryValue, true);

        }

        public IEditableValueViewModel<IFormattedValue> EditableValueViewModel { get; }
    }
}