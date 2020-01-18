using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.ViewModels.Fragment.FragmentOptions
{
   public class DefaultFragmentOptionsViewModel: IFragmentOptionsViewModel
    {

        public DefaultFragmentOptionsViewModel()
        {
            FragmentOptionGroupViewModels=new ObservableCollection<IFragmentOptionGroupViewModel>();
        }


        public ObservableCollection<IFragmentOptionGroupViewModel> FragmentOptionGroupViewModels { get; set; }
    }
}
