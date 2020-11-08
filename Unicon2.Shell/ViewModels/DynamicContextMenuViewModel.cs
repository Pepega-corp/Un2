using Prism.Mvvm;
using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Shell.ViewModels
{
    public class DynamicContextMenuViewModel : BindableBase
    {
        public DynamicContextMenuViewModel()
        {

        }

        public List<IFragmentOptionGroupViewModel> FragmentOptionGroupViewModels { get; set; }
    }
}