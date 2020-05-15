using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.ViewModel.Presentation;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.ViewModel
{
    public class MeasuringGroupViewModel : ViewModelBase, IMeasuringGroupViewModel
    {
        private List<PresentationMeasuringElementViewModel> _presentationMeasuringElements;

        public MeasuringGroupViewModel()
        {
            MeasuringElementViewModels = new ObservableCollection<IMeasuringElementViewModel>();
            PresentationMeasuringElements=new List<PresentationMeasuringElementViewModel>();
        }

        public string StrongName => MeasuringKeys.MEASURING_GROUP +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
       
        public List<PresentationMeasuringElementViewModel> PresentationMeasuringElements
        {
	        get => _presentationMeasuringElements;
	        set
	        {
		        _presentationMeasuringElements = value;
                RaisePropertyChanged();
	        }
        }

        public ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }

        public string Header { get; set; }
        public async Task LoadGroup()
        {
	        throw new System.NotImplementedException();
        }
    }
}
