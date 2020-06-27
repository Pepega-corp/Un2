using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty
{
	public class EditableDependentValueSetUnchangedSubscription : IDeviceSubscription
	{
		private readonly ILocalAndDeviceValueContainingViewModel _localAndDeviceValueContainingViewModel;
		private readonly IDeviceMemory _deviceMemory;
		private readonly ushort _address;
		private readonly ushort _numberOfPoints;

		public EditableDependentValueSetUnchangedSubscription(ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,
			IDeviceMemory deviceMemory, ushort address, ushort numberOfPoints)
		{
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

				if (_deviceMemory.DeviceMemoryValues[i] == _deviceMemory.LocalMemoryValues[i]) continue;
				isMemoryEqualOnAddresses = false;
				break;
			}

			if (_localAndDeviceValueContainingViewModel?.LocalValue != null)
				_localAndDeviceValueContainingViewModel.LocalValue.IsFormattedValueChanged = !isMemoryEqualOnAddresses;
		}

	}
}
