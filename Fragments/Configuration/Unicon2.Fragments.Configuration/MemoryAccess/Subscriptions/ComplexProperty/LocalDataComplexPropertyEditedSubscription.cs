using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
	class LocalDataComplexPropertyEditedSubscription : ILocalDataMemorySubscription
	{
		private readonly IRuntimeComplexPropertyViewModel _complexPropertyViewModel;
		private readonly DeviceContext _deviceContext;
		private readonly IComplexProperty _complexProperty;
		private readonly int _offset;

		public LocalDataComplexPropertyEditedSubscription(IRuntimeComplexPropertyViewModel complexPropertyViewModel,
			DeviceContext deviceContext, IComplexProperty complexProperty,int offset)
		{
			_complexPropertyViewModel = complexPropertyViewModel;
			_deviceContext = deviceContext;
			_complexProperty = complexProperty;
			_offset = offset;
		}

		public void Execute()
		{
			IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();
			IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor = StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();

			//var resultBitArray=new bool[16];
            var resultBitArray = _deviceContext.DeviceMemory.LocalMemoryValues[_complexProperty.Address].GetBoolArrayFromUshort();


			foreach (var subProperty in _complexProperty.SubProperties)
			{
				var ushorts = formattingService.FormatBack(subProperty?.UshortsFormatter, (_complexPropertyViewModel.ChildStructItemViewModels[_complexProperty.SubProperties.IndexOf(subProperty)] as ILocalAndDeviceValueContainingViewModel).LocalValue.Accept(fetchingFromViewModelVisitor));
				var ushortOfSubProperty = ushorts.First();
				var boolArray = ushorts.GetBoolArrayFromUshortArray();
				int counter = 0;
				for (int i = 0; i < 16; i++)
				{
					if (subProperty.BitNumbersInWord.Contains(i))
					{
						resultBitArray[i] = boolArray[counter];
						counter++;
					}

					
				}

			}

			resultBitArray = resultBitArray.ToArray();
			var resUshorts = new ushort[] {resultBitArray.BoolArrayToUshort()};
			MemoryAccessor.SetUshortsInMemory(_deviceContext.DeviceMemory, (ushort)(_complexProperty.Address+_offset), resUshorts, true);
			_deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
				(ushort) (_complexProperty.Address + _offset), (ushort) resUshorts.Length);
		}
        public int Priority { get; set; } = 2;

	}
}