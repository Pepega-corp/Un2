using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Factories
{
    public class MeasuringGroupEditorViewModelFactory : IMeasuringGroupEditorViewModelFactory
    {
        private readonly ITypesContainer _container;
        private readonly IMeasuringElementEditorViewModelFactory _measuringElementEditorViewModelFactory;

        public MeasuringGroupEditorViewModelFactory(ITypesContainer container,
            IMeasuringElementEditorViewModelFactory measuringElementEditorViewModelFactory)
        {
            _container = container;
            _measuringElementEditorViewModelFactory = measuringElementEditorViewModelFactory;
        }

        public IMeasuringGroupEditorViewModel CreateMeasuringGroupEditorViewModel(IMeasuringGroup measuringGroup = null)
        {
            if (measuringGroup == null)
            {
                measuringGroup = _container.Resolve<IMeasuringGroup>();
                measuringGroup.Name = "New";
            }

            IMeasuringGroupEditorViewModel measuringGroupEditorViewModel =
                _container.Resolve<IMeasuringGroupEditorViewModel>();

            foreach (IMeasuringElement measuringElement in measuringGroup.MeasuringElements)
            {
                measuringGroupEditorViewModel.MeasuringElementEditorViewModels.Add(
                    _measuringElementEditorViewModelFactory.CreateMeasuringElementEditorViewModel(measuringElement));
            }
            measuringGroupEditorViewModel.PresentationSettingsViewModel = new PresentationSettingsViewModelFactory()
	            .CreatePresentationSettingsViewModel(measuringGroup, measuringGroupEditorViewModel);
            measuringGroupEditorViewModel.Header = measuringGroup.Name;
            return measuringGroupEditorViewModel;
        }

    }
}