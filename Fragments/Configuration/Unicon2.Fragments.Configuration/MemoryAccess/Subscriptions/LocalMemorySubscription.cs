using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class LocalMemorySubscription : IDeviceSubscription
    {
        private readonly IEditableValueViewModel _editableValueViewModel;
        private readonly ushort _address;
        private readonly ushort _numberOfPoints;
        private readonly IUshortsFormatter _ushortsFormatter;
        private readonly IDeviceMemory _deviceMemory;
        private ushort[] _prevUshorts=new ushort[0];
        public LocalMemorySubscription(IEditableValueViewModel editableValueViewModel,
            ushort address, ushort numberOfPoints, IUshortsFormatter ushortsFormatter, IDeviceMemory deviceMemory)
        {
            _editableValueViewModel = editableValueViewModel;
            _address = address;
            _numberOfPoints = numberOfPoints;
            _ushortsFormatter = ushortsFormatter;
            _deviceMemory = deviceMemory;
        }

        public void Execute()
        {
			var newUshorts=MemoryAccessor.GetUshortsFromMemory(_deviceMemory, _address, _numberOfPoints, true);
			if (!newUshorts.IsEqual(_prevUshorts))
			{
				_prevUshorts = newUshorts;
				var localValue = StaticContainer.Container.Resolve<IFormattingService>().FormatValue(_ushortsFormatter,
					_prevUshorts);
				_editableValueViewModel.Accept(new EditableValueSetFromLocalVisitor(localValue));
			}

			
        }

      
    }
}