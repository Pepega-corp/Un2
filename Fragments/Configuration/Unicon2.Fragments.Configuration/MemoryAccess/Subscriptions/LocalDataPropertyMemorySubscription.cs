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

        public LocalDataPropertyMemorySubscription(IEditableValueViewModel editableValueViewModel, IConfigurationMemory configurationMemory, IProperty property)
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
                    _property?.UshortsFormatter.FormatBack(property.LocalValue.Model as IFormattedValue);
                MemoryAccessor.GetUshortsInMemory(_configurationMemory,propertyModel.Address,memoryValue,_isLocal);
            
        }

        public IEditableValueViewModel EditableValueViewModel { get; }
    }
}