using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public class DiscretMeasuringElementEditorViewModel : MeasuringElementEditorViewModelBase, IDiscretMeasuringElementEditorViewModel
    {
        private IBitAddressEditorViewModel _bitAddressEditorViewModel;

        public DiscretMeasuringElementEditorViewModel(IBitAddressEditorViewModel bitAddressEditorViewModel)
        {
            this._bitAddressEditorViewModel = bitAddressEditorViewModel;
        }

        public override string NameForUiKey => MeasuringKeys.DISCRET_MEASURING_ELEMENT;


        public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


        public IBitAddressEditorViewModel BitAddressEditorViewModel
        {
            get { return this._bitAddressEditorViewModel; }
            set
            {
                this._bitAddressEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }


        protected override void SetModel(object value)
        {
            base.SetModel(value);
            if ((this._measuringElement as IDiscretMeasuringElement).AddressOfBit != null)
            {
                this.BitAddressEditorViewModel.Model = (this._measuringElement as IDiscretMeasuringElement).AddressOfBit;
            }
        }

        protected override IMeasuringElement GetModel()
        {
            (this._measuringElement as IDiscretMeasuringElement).AddressOfBit = this.BitAddressEditorViewModel.Model as IAddressOfBit;
            return base.GetModel();
        }
    }
}
