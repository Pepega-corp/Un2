using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
   public class EditableValueSetUnchangedSubscription:IMemorySubscription
    {
        private readonly IEditableValueViewModel _editableValueViewModel;
        private readonly IDeviceMemory _deviceMemory;
        private readonly ushort _address;
        private readonly ushort _numberOfPoints;

        public EditableValueSetUnchangedSubscription(IEditableValueViewModel editableValueViewModel,IDeviceMemory deviceMemory,ushort address, ushort numberOfPoints)
        {
            _editableValueViewModel = editableValueViewModel;
            _deviceMemory = deviceMemory;
            _address = address;
            _numberOfPoints = numberOfPoints;
        }

        public void Execute()
        {
            var isMemoryEqualOnAddresses = true;
            for (ushort i = _address; i < _address + _numberOfPoints; i++)
            {
                if (!_deviceMemory.LocalMemoryValues.ContainsKey(i) || !_deviceMemory.DeviceMemoryValues.ContainsKey(i))
                {
                    isMemoryEqualOnAddresses = false;
                    break;
                }
                if (_deviceMemory.DeviceMemoryValues[i] == _deviceMemory.LocalMemoryValues[i]) continue;
                isMemoryEqualOnAddresses = false;
                break;
            }

            if (isMemoryEqualOnAddresses)
            {
                _editableValueViewModel.RefreshBaseValueToCompare();
            }
        }
        
    }
}
