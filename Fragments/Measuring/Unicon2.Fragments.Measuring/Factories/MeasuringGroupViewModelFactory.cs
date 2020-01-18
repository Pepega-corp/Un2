using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Factories
{
    public class MeasuringGroupViewModelFactory : IMeasuringGroupViewModelFactory
    {
        private readonly ITypesContainer _container;

        public MeasuringGroupViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }


        public IMeasuringGroupViewModel CreateMeasuringGroupViewModel(IMeasuringGroup measuringGroup)
        {
            IMeasuringGroupViewModel measuringGroupViewModel = this._container.Resolve<IMeasuringGroupViewModel>();
            measuringGroupViewModel.Model = measuringGroup;
            return measuringGroupViewModel;
        }
    }
}
