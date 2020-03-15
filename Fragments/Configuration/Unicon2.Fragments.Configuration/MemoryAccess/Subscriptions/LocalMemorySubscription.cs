using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class LocalMemorySubscription : IMemorySubscription
    {
        private readonly IEditableValueViewModel _editableValueViewModel;
        private readonly ushort _address;
        private readonly ushort _numberOfPoints;
        private readonly IUshortsFormatter _ushortsFormatter;
        private readonly IDeviceMemory _deviceMemory;
        private readonly EditableValueSetUnchangedSubscription _dependency;

        public LocalMemorySubscription(IEditableValueViewModel editableValueViewModel,
            ushort address, ushort numberOfPoints, IUshortsFormatter ushortsFormatter, IDeviceMemory deviceMemory, EditableValueSetUnchangedSubscription dependency)
        {
            _editableValueViewModel = editableValueViewModel;
            _address = address;
            _numberOfPoints = numberOfPoints;
            _ushortsFormatter = ushortsFormatter;
            _deviceMemory = deviceMemory;
            _dependency = dependency;
        }

        public void Execute()
        {
            var localValue = StaticContainer.Container.Resolve<IFormattingService>().FormatValue(_ushortsFormatter,
                MemoryAccessor.GetUshortsFromMemory(_deviceMemory, _address, _numberOfPoints, true));
            _editableValueViewModel.Accept(new EditableValueSetFromLocalVisitor(localValue));
            _dependency.Execute();
        }
    }
}