using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Subscription;
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
            DeviceContext deviceContext, IProperty property,int offset)
        {
	        _deviceContext = deviceContext;
	        _property = property;
            _offset = offset;
            EditableValueViewModel = editableValueViewModel;
        }

        public void Execute()
        {
            IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();
            IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor = StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();

            var ushorts = formattingService.FormatBack(_property?.UshortsFormatter, EditableValueViewModel.Accept(fetchingFromViewModelVisitor));
            
            MemoryAccessor.GetUshortsInMemory(_deviceContext.DeviceMemory, (ushort)(_property.Address + _offset), ushorts, true);
            _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
	            (ushort) (_property.Address + _offset), (ushort) ushorts.Length);
        }

        public IEditableValueViewModel EditableValueViewModel { get; }
    }
}