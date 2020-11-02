using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
   public class SubPropertySetUnchangedSubscription : IDeviceSubscription
    {
        private readonly List<int> _bitNumbersInWord;
        private readonly ILocalAndDeviceValueContainingViewModel _localAndDeviceValueContainingViewModel;
        private readonly IDeviceMemory _deviceMemory;
        private readonly ushort _address;
        private readonly ushort _numberOfPoints;
        public int Priority { get; set; } = 1;

        public SubPropertySetUnchangedSubscription(List<int> bitNumbersInWord,ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,
        IDeviceMemory deviceMemory, ushort address, ushort numberOfPoints)
        {
            _bitNumbersInWord = bitNumbersInWord;
            _localAndDeviceValueContainingViewModel = localAndDeviceValueContainingViewModel;
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
                    break;
                }

                foreach (var bitNumber in _bitNumbersInWord)
                {
                    var boolArrayDevice = _deviceMemory.DeviceMemoryValues[i].GetBoolArrayFromUshort();
                    var boolArrayLocal = _deviceMemory.LocalMemoryValues[i].GetBoolArrayFromUshort();


                    if (boolArrayDevice[bitNumber] == boolArrayLocal[bitNumber]) continue;
                    isMemoryEqualOnAddresses = false;
                    break;
                }

                if (!isMemoryEqualOnAddresses)
                {
                    break;
                }

            
            }

            _localAndDeviceValueContainingViewModel.LocalValue.IsFormattedValueChanged = !isMemoryEqualOnAddresses;
        }
    }
}
