using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Factories
{
    public class FragmentEditorViewModelFactory : IFragmentEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public FragmentEditorViewModelFactory(ITypesContainer container)
        {
            _container = container;
        }

        public IFragmentEditorViewModel CreateFragmentEditorViewModel(IDeviceFragment deviceFragment)
        {
            IFragmentEditorViewModel fragmentEditorViewModel = _container.Resolve<IFragmentEditorViewModel>(deviceFragment.StrongName +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            fragmentEditorViewModel.Initialize(deviceFragment);
            return fragmentEditorViewModel;
        }
    }
}
