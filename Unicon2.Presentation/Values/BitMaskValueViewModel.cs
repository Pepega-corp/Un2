using System.Collections.Generic;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
	public class BitMaskValueViewModel : FormattableValueViewModelBase, IBitMaskValueViewModel
	{
		private List<List<bool>> _bitArray;

		public override string StrongName => PresentationKeys.BIT_MASK_VALUE +
		                                     ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

		public override string AsString()
		{
			return nameof(BitMaskValueViewModel);
		}

		public List<List<bool>> BitArray
		{
			get { return _bitArray; }
			set { SetProperty(ref _bitArray, value); }
		}
	}
}