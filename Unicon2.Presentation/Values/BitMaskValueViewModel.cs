using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Values;
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

        public override void InitFromValue(IFormattedValue value)
        {
            if (value is IBitMaskValue bitMaskValue)
            {
                this.BitArray = bitMaskValue.BitArray;
            }
            base.InitFromValue(value);
        }

        public List<List<bool>> BitArray
        {
            get { return this._bitArray; }
            set { SetProperty(ref this._bitArray, value); }
        }
    }
}