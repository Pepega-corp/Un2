using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Fragments.GraphicalMenu.ViewModel
{
    public class GraphicalMenuFragmentViewModel : IFragmentViewModel
    {
        public void Initialize(IDeviceFragment deviceFragment)
        {
            
        }

        public string StrongName => GraphicalMenuKeys.GRAPHICAL_MENU +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public string NameForUiKey => GraphicalMenuKeys.GRAPHICAL_MENU;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
    }
}
