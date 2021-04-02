using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Model;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.GraphicalMenu.Editor.ViewModel
{
    public class GraphicalMenuFragmentEditorViewModel : IGraphicalMenuFragmentEditorViewModel
    {
        private readonly Func<IGraphicalMenu> _graphicalMenuFactory;

        public GraphicalMenuFragmentEditorViewModel(Func<IGraphicalMenu> graphicalMenuFactory)
        {
            _graphicalMenuFactory = graphicalMenuFactory;
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            
        }

        public string StrongName => GraphicalMenuKeys.GRAPHICAL_MENU +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public string NameForUiKey => GraphicalMenuKeys.GRAPHICAL_MENU;

        public IDeviceFragment BuildDeviceFragment()
        {
            return _graphicalMenuFactory();
        }
    }
}
