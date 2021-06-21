using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
	public class LocalComplexPropertyMemorySubscription : IDeviceSubscription
	{
		private readonly IRuntimeComplexPropertyViewModel _runtimeComplexPropertyViewModel;
		private readonly IComplexProperty _complexProperty;
		private readonly IDeviceMemory _deviceMemory;
		private readonly int _offset;
		private ushort[] _prevUshorts=new ushort[0];

        public int Priority { get; set; } = 1;

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
				bool[] subPropertyBools = new bool[16];
				int counter = 0;
				for (int i = 0; i < 16; i++)
				{
					if (subProperty.BitNumbersInWord.Contains(i))
					{
						subPropertyBools[counter] = boolArray[i];
						counter++;
					}
				}
				

				var subPropertyUshort = subPropertyBools.BoolArrayToUshort();


				var subPropertyValue = StaticContainer.Container.Resolve<IFormattingService>().FormatValue(subProperty.UshortsFormatter,
					new []{subPropertyUshort},true);


				var subPropertyViewModel =
					_runtimeComplexPropertyViewModel.ChildStructItemViewModels[
						_complexProperty.SubProperties.IndexOf(subProperty)];
				(subPropertyViewModel as ILocalAndDeviceValueContainingViewModel).LocalValue.Accept(new EditableValueSetFromLocalVisitor(subPropertyValue));
			}

		}
	}
}