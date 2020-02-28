using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IFragmentPaneViewModelFactory
    {
        IFragmentPaneViewModel GetFragmentPaneViewModel(IFragmentViewModel fragmentViewModel,
            IEnumerable<IDeviceViewModel> deviceViewModels);
    }
}