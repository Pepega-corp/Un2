using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Shell.ViewModels
{
    public class DynamicContextMenuViewModel:BindableBase
    {
        public DynamicContextMenuViewModel()
        {

        }

        public List<IFragmentOptionGroupViewModel> FragmentOptionGroupViewModels { get; set; }
    }
}
