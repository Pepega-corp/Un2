using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public class DiscretMeasuringElementEditorViewModel : MeasuringElementEditorViewModelBase, IDiscretMeasuringElementEditorViewModel
    {
        private IBitAddressEditorViewModel _bitAddressEditorViewModel;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;

        public DiscretMeasuringElementEditorViewModel(IBitAddressEditorViewModel bitAddressEditorViewModel,ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
        {
            _bitAddressEditorViewModel = bitAddressEditorViewModel;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            AddAsResourceCommand = new RelayCommand(OnAddAsResource);
        }

        private void OnAddAsResource()
        {
	        _sharedResourcesGlobalViewModel.AddAsSharedResourceWithContainer(this);
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

        public object AddAsResourceCommand { get; }
    }
}
