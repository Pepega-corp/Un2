using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class EditableValueSetUnchangedSubscription : IDeviceSubscription
    {
        private readonly ILocalAndDeviceValueContainingViewModel _localAndDeviceValueContainingViewModel;
        private readonly IDeviceMemory _deviceMemory;
        private readonly ushort _address;
        private readonly ushort _numberOfPoints;
        public int Priority { get; set; } = 10;
        public IBitConfigurable _bitConfig;

        public EditableValueSetUnchangedSubscription(
            ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,
            IDeviceMemory deviceMemory, ushort address, ushort numberOfPoints, IBitConfigurable bitConfig)
        {
            _localAndDeviceValueContainingViewModel = localAndDeviceValueContainingViewModel;
            _deviceMemory = deviceMemory;
            _address = address;
            _numberOfPoints = numberOfPoints;
            _bitConfig = bitConfig;
        }

        public void Execute()
        {
            /*
            var isMemoryEqualOnAddresses = true;

            for (ushort i = _address; i < _address + _numberOfPoints; i++)
            {
                if (!_deviceMemory.LocalMemoryValues.ContainsKey(i) || !_deviceMemory.DeviceMemoryValues.ContainsKey(i))
                {
                    break;
                }

                if (_bitConfig.IsFromBits)
                {
                    foreach (var bitNumber in _bitConfig.BitNumbers)
                    {
                        var boolArrayDevice = _deviceMemory.DeviceMemoryValues[i].GetBoolArrayFromUshort();
                        var boolArrayLocal = _deviceMemory.LocalMemoryValues[i].GetBoolArrayFromUshort();


                        if (boolArrayDevice[bitNumber] == boolArrayLocal[bitNumber]) continue;
                        isMemoryEqualOnAddresses = false;
                        break;
                    }
                }
                else
                {
                    if (_deviceMemory.DeviceMemoryValues[i] == _deviceMemory.LocalMemoryValues[i]) continue;
                    isMemoryEqualOnAddresses = false;
                }

                if (!isMemoryEqualOnAddresses)
                {
                    break;
                }

            }

            */

            if (_localAndDeviceValueContainingViewModel?.LocalValue != null&& _localAndDeviceValueContainingViewModel?.DeviceValue != null)
                _localAndDeviceValueContainingViewModel.LocalValue.IsFormattedValueChanged = _localAndDeviceValueContainingViewModel.LocalValue.Accept(
                    new EditableValueIsChangedVisitor(_localAndDeviceValueContainingViewModel.DeviceValue));


        }

    }
}
