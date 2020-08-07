using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
	public class DeviceDataComplexPropertySubscription : IDeviceDataMemorySubscription
	{
		private readonly IComplexProperty _complexProperty;
		private readonly IRuntimeComplexPropertyViewModel _runtimeComplexPropertyViewModel;
		private readonly ushort _offset;
		private readonly IValueViewModelFactory _valueViewModelFactory;
		private readonly IDeviceMemory _deviceMemory;

		public DeviceDataComplexPropertySubscription(
			IValueViewModelFactory valueViewModelFactory, IDeviceMemory deviceMemory, IComplexProperty complexProperty,
			IRuntimeComplexPropertyViewModel runtimeComplexPropertyViewModel, ushort offset)
		{
			_valueViewModelFactory = valueViewModelFactory;
			_offset = offset;
			_deviceMemory = deviceMemory;
			_complexProperty = complexProperty;
			_runtimeComplexPropertyViewModel = runtimeComplexPropertyViewModel;
		}

		public void Execute()
		{
			IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();

			var ushortsFromDevice = MemoryAccessor.GetUshortsFromMemory(
				_deviceMemory,
				(ushort) (_complexProperty.Address + _offset), _complexProperty.NumberOfPoints, false);
			foreach (var subProperty in _complexProperty.SubProperties)
			{
				var boolArray = ushortsFromDevice.GetBoolArrayFromUshortArray().ToArray();
				List<bool> subPropertyBools = new List<bool>();
				foreach (var bitNumber in subProperty.BitNumbersInWord)
				{
					subPropertyBools.Add(boolArray[bitNumber]);
				}
				subPropertyBools.Reverse();
				var subPropertyUshort = subPropertyBools.BoolArrayToUshort();
				var subPropertyValue =
					formattingService.FormatValue(subProperty.UshortsFormatter, new[] {subPropertyUshort});
				var subPropertyViewModel =
					_runtimeComplexPropertyViewModel.ChildStructItemViewModels[
						_complexProperty.SubProperties.IndexOf(subProperty)];
				(subPropertyViewModel as ILocalAndDeviceValueContainingViewModel).DeviceValue =
					_valueViewModelFactory.CreateFormattedValueViewModel(subPropertyValue);
			}
		}
	}
}