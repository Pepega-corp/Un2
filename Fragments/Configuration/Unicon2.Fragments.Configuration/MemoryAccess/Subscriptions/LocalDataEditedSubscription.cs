using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class LocalDataEditedSubscription : ILocalDataMemorySubscription
    {
        private readonly DeviceContext _deviceContext;
        private readonly IProperty _property;
        private readonly int _offset;

        public LocalDataEditedSubscription(IEditableValueViewModel editableValueViewModel,
            DeviceContext deviceContext, IProperty property, int offset)
        {
            _deviceContext = deviceContext;
            _property = property;
            _offset = offset;
            EditableValueViewModel = editableValueViewModel;
        }
        public int Priority { get; set; } = 2;

        public void Execute()
        {
            IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();
            IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor =
                StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();


            var formatterForDependentProperty = _property.UshortsFormatter;

            if (_property?.Dependencies?.Count > 0)
            {
                formatterForDependentProperty =
                    DependentSubscriptionHelpers.GetFormatterConsideringDependencies(_property.Dependencies,_deviceContext, formattingService,_property.UshortsFormatter,(ushort)_offset);
            }


            var ushorts = formattingService.FormatBack(formatterForDependentProperty,
                EditableValueViewModel.Accept(fetchingFromViewModelVisitor));

            MemoryAccessor.SetUshortsInMemory(_deviceContext.DeviceMemory, (ushort) (_property.Address + _offset),
                ushorts, true);
            _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
                (ushort) (_property.Address + _offset), (ushort) ushorts.Length);
        }

     



        public IEditableValueViewModel EditableValueViewModel { get; }
    }
}