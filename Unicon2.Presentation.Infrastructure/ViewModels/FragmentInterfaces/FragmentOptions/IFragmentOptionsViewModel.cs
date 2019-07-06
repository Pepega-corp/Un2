using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions
{
    public interface IFragmentOptionsViewModel
    {
        ObservableCollection<IFragmentOptionGroupViewModel> FragmentOptionGroupViewModels { get; set; }
    }
}