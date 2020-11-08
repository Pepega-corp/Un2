using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;

namespace Unicon2.Fragments.Measuring.Editor.Factories
{
	public class BitAddressEditorViewModelFactory
	{
		public IBitAddressEditorViewModel CreateBitAddressEditorViewModel(IAddressOfBit addressOfBit = null)
		{
			if (addressOfBit == null)
			{
				return new BitAddressEditorViewModel()
				{
					BitNumberInWord = 0,
					Address = 0,
					FunctionNumber = 0
				};
			}
			return new BitAddressEditorViewModel()
			{
				BitNumberInWord = addressOfBit.BitAddressInWord,
				Address = addressOfBit.Address,
				FunctionNumber = addressOfBit.NumberOfFunction
			};
		}
	}
}
