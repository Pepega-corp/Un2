using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Factories
{
    public class MeasuringElementViewModelFactory : IMeasuringElementViewModelFactory
    {
        private readonly ITypesContainer _container;

        public MeasuringElementViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }
        #region Implementation of IMeasuringElementViewModelFactory

        public IMeasuringElementViewModel CreateMeasuringElementViewModel(IMeasuringElement measuringElement, string gruopName)
        {
            IMeasuringElementViewModel measuringElementViewModel =
                this._container.Resolve<IMeasuringElementViewModel>(measuringElement.StrongName +
                                                               ApplicationGlobalNames.CommonInjectionStrings
                                                                   .VIEW_MODEL);
            measuringElementViewModel.Model = measuringElement;
            measuringElementViewModel.GroupName = gruopName;
            return measuringElementViewModel;
        }


        #endregion
    }
}
