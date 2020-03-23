using System.Collections.ObjectModel;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.ViewModel
{
    public class MeasuringGroupViewModel : ViewModelBase, IMeasuringGroupViewModel
    {
        private readonly IMeasuringElementViewModelFactory _measuringElementViewModelFactory;
        private IMeasuringGroup _measuringGroup;

        public MeasuringGroupViewModel(IMeasuringElementViewModelFactory measuringElementViewModelFactory)
        {
            _measuringElementViewModelFactory = measuringElementViewModelFactory;
            MeasuringElementViewModels = new ObservableCollection<IMeasuringElementViewModel>();
        }

        public string StrongName => MeasuringKeys.MEASURING_GROUP +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return GetModel(); }
            set { SetModel(value); }
        }

        private void SetModel(object value)
        {
            _measuringGroup = value as IMeasuringGroup;
            Header = _measuringGroup.Name;
            MeasuringElementViewModels.Clear();
            foreach (IMeasuringElement measuringElement in _measuringGroup.MeasuringElements)
            {
                MeasuringElementViewModels.Add(_measuringElementViewModelFactory.CreateMeasuringElementViewModel(measuringElement, Header));
            }
        }

        private object GetModel()
        {
            _measuringGroup.MeasuringElements.Clear();
            foreach (IMeasuringElementViewModel measuringElementViewModel in MeasuringElementViewModels)
            {
	            //this._measuringGroup.MeasuringElements.Add(measuringElementViewModel.Model as IMeasuringElement);
            }
            return _measuringGroup;
        }

        public ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }

        public string Header { get; private set; }
    }
}
