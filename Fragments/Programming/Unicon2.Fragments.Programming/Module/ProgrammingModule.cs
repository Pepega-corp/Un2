using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Fragments.Programming.ViewModels;
using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Module
{
    public class ProgrammingModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IProgrammModel, ProgrammModel>();

            container.Register<IFragmentViewModel, ProgrammingViewModel>(ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<ISchemeTabViewModel, SchemeTabViewModel>(ProgrammingKeys.SCHEME_TAB + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<IElementLibraryModel, ElementLibraryModel>(ProgrammingKeys.ELEMENT_LIBRARY);
            container.Register<IElementLibraryViewModel, ElementLibraryViewModel>(ProgrammingKeys.ELEMENT_LIBRARY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<ILogicElement, Input>(ProgrammingKeys.INPUT);
            container.Register<ILogicElementViewModel, InputViewModel>(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            //container.Register<ILogicElement, Output>(ProgrammingKeys.OUTPUT);
            //container.Register<ILogicElementViewModel, OutputViewModel>(ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
        
            container.Resolve<ISerializerService>().AddKnownTypeForSerialization(typeof(ProgrammModel));
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/UI/ProgrammingViewTemplate.xaml", GetType().Assembly);
        }
    }
}
