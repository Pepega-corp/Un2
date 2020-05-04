using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
	public class LocalComplexPropertyMemorySubscription : IMemorySubscription
	{
		private readonly IRuntimeComplexPropertyViewModel _runtimeComplexPropertyViewModel;
		private readonly IComplexProperty _complexProperty;
		private readonly IDeviceMemory _deviceMemory;
		private readonly int _offset;
		private ushort[] _prevUshorts=new ushort[0];


		public LocalComplexPropertyMemorySubscription(IRuntimeComplexPropertyViewModel runtimeComplexPropertyViewModel,IComplexProperty complexProperty, IDeviceMemory deviceMemory,int offset)
		{
			_runtimeComplexPropertyViewModel = runtimeComplexPropertyViewModel;
			_complexProperty = complexProperty;
			_deviceMemory = deviceMemory;
			_offset = offset;
		}

		public void Execute()
		{
			var ushortsFromDevice = MemoryAccessor.GetUshortsFromMemory(_deviceMemory, (ushort)(_complexProperty.Address + _offset),
				_complexProperty.NumberOfPoints, true);
			if (_prevUshorts.IsEqual(ushortsFromDevice))
			{
				return;
			}
			_prevUshorts = ushortsFromDevice;
			foreach (var subProperty in _complexProperty.SubProperties)
			{
				var boolArray = ushortsFromDevice.GetBoolArrayFromUshortArray();
				List<bool> subPropertyBools = new List<bool>();
				foreach (var bitNumber in subProperty.BitNumbersInWord)
				{
					subPropertyBools.Add(boolArray[bitNumber]);
				}

				var subPropertyUshort = subPropertyBools.BoolArrayToUshort();


				var subPropertyValue = StaticContainer.Container.Resolve<IFormattingService>().FormatValue(subProperty.UshortsFormatter,
					new []{subPropertyUshort});


				var subPropertyViewModel =
					_runtimeComplexPropertyViewModel.ChildStructItemViewModels[
						_complexProperty.SubProperties.IndexOf(subProperty)];
				(subPropertyViewModel as ILocalAndDeviceValueContainingViewModel).LocalValue.Accept(new EditableValueSetFromLocalVisitor(subPropertyValue));
			}

		}
	}
}