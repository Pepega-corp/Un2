using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.GraphicalMenu.Editor.ViewModel;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.GraphicalMenu.Editor.Module
{
    public class GraphicalMenuEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IFragmentEditorViewModel), typeof(GraphicalMenuFragmentEditorViewModel),
                GraphicalMenuKeys.GRAPHICAL_MENU +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/GraphicalMenuDataTemplates.xaml",
                GetType().Assembly);

        }
    }
}
