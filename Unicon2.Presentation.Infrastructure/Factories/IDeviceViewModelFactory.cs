using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IDeviceViewModelFactory
    {
        Task<Result<IDeviceViewModel>> CreateDeviceViewModel(IDevice device);
    }
}