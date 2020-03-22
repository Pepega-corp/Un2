using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure.Common;

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
