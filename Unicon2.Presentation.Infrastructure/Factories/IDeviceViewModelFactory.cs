using System;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IDeviceViewModelFactory
    {
        IDeviceViewModel CreateDeviceViewModel(IDevice device, Func<IFragmentViewModel> getActiveFragment);
    }
}