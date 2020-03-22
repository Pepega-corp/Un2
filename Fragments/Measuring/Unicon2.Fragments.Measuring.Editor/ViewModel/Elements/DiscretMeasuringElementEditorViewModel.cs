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
            _bitAddressEditorViewModel = bitAddressEditorViewModel;
        }

        public override string NameForUiKey => MeasuringKeys.DISCRET_MEASURING_ELEMENT;


        public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


        public IBitAddressEditorViewModel BitAddressEditorViewModel
        {
            get { return _bitAddressEditorViewModel; }
            set
            {
                _bitAddressEditorViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}
