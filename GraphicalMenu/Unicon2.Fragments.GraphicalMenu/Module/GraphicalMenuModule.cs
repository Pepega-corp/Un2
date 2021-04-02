
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Model;
using Unicon2.Fragments.GraphicalMenu.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.GraphicalMenu.Module
{
    public class GraphicalMenuModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IGraphicalMenu, Model.GraphicalMenu>();

            container.Register<IFragmentViewModel, GraphicalMenuFragmentViewModel>(
                GraphicalMenuKeys.GRAPHICAL_MENU + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/GraphicalMenuDataTemplates.xaml",
                GetType().Assembly);
        }
    }
}
