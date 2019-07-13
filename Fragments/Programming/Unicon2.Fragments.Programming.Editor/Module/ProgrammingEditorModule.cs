using Unicon2.Fragments.Programming.Editor.Factories;
using Unicon2.Fragments.Programming.Editor.Interfaces;
using Unicon2.Fragments.Programming.Editor.ViewModel;
using Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Editor.Module
{
    public class ProgrammingEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IFragmentEditorViewModel, ProgrammingEditorViewModel>
                (ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Register<ILogicElementFactory, LogicElementFactory>();
            //All view models
            container.Register<ILogicElementEditorViewModel, InputEditorViewModel>(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/ProgrammingEditorTemplate.xaml", GetType().Assembly);
        }
    }
}
