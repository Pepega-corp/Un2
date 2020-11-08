using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Measuring.Editor.Helpers
{
	public class BitAddressSaver
	{
		public IAddressOfBit GetAddressOfBitFromEditor(IBitAddressEditorViewModel bitAddressEditorViewModel)
		{
			var result = StaticContainer.Container.Resolve<IAddressOfBit>();
			result.Address = bitAddressEditorViewModel.Address;
			result.BitAddressInWord = bitAddressEditorViewModel.BitNumberInWord;
			result.NumberOfFunction = bitAddressEditorViewModel.FunctionNumber;
			return result;
		}
	}
}
