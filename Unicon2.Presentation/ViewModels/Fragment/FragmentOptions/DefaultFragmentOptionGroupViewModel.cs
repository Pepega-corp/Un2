using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.ViewModels.Fragment.FragmentOptions
{
    public class DefaultFragmentOptionGroupViewModel : IFragmentOptionGroupViewModel
    {

        public DefaultFragmentOptionGroupViewModel()
        {
            this.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();
        }


        #region Implementation of IFragmentOptionGroupViewModel

        public string NameKey { get; set; }

        public List<IFragmentOptionCommandViewModel> FragmentOptionCommandViewModels { get; set; }

        #endregion
    }
}
