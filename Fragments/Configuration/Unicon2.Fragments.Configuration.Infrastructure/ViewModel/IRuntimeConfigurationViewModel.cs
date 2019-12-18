using System.Collections.ObjectModel;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel
{
    public interface IRuntimeConfigurationViewModel:IFragmentViewModel,IDeviceDataProvider
    {

        ObservableCollection<IRuntimeConfigurationItemViewModel> RootConfigurationItemViewModels { get; set; }
        ObservableCollection<IRuntimeConfigurationItemViewModel> AllRows { get; set; }
        
    }
}