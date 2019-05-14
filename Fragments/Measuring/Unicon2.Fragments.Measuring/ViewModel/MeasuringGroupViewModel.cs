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
            this._measuringElementViewModelFactory = measuringElementViewModelFactory;
            this.MeasuringElementViewModels = new ObservableCollection<IMeasuringElementViewModel>();
        }

        #region Implementation of IStronglyNamed

        public string StrongName => MeasuringKeys.MEASURING_GROUP +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            this._measuringGroup = value as IMeasuringGroup;
            this.Header = this._measuringGroup.Name;
            this.MeasuringElementViewModels.Clear();
            foreach (IMeasuringElement measuringElement in this._measuringGroup.MeasuringElements)
            {
                this.MeasuringElementViewModels.Add(this._measuringElementViewModelFactory.CreateMeasuringElementViewModel(measuringElement, this.Header));
            }
        }

        private object GetModel()
        {
            this._measuringGroup.MeasuringElements.Clear();
            foreach (IMeasuringElementViewModel measuringElementViewModel in this.MeasuringElementViewModels)
            {
                this._measuringGroup.MeasuringElements.Add(measuringElementViewModel.Model as IMeasuringElement);
            }
            return this._measuringGroup;
        }

        #endregion

        #region Implementation of IMeasuringGroupViewModel

        public ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }

        public string Header { get; private set; }

        #endregion
    }
}
