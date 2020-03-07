using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimeConfigurationViewModel : IFragmentViewModel, IDeviceDataProvider, IStronglyNamed
    {

        ObservableCollection<IRuntimeConfigurationItemViewModel> RootConfigurationItemViewModels { get; set; }
        ObservableCollection<IRuntimeConfigurationItemViewModel> AllRows { get; set; }
        IDeviceEventsDispatcher DeviceEventsDispatcher { get; }
		IDeviceMemory DeviceMemory { get; }
    }
}