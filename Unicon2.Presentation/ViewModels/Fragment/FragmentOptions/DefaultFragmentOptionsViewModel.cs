using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.ViewModels.Fragment.FragmentOptions
{
   public class DefaultFragmentOptionsViewModel: IFragmentOptionsViewModel
    {

        public DefaultFragmentOptionsViewModel()
        {
            FragmentOptionGroupViewModels=new List<IFragmentOptionGroupViewModel>();
        }


        #region Implementation of IFragmentOptionsViewModel

        public List<IFragmentOptionGroupViewModel> FragmentOptionGroupViewModels { get; set; }

        #endregion
    }
}
