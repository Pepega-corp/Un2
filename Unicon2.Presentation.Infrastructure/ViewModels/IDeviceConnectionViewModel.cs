using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IDeviceConnectionViewModel:IViewModel
    {
        string ConnectionName { get; }
    }
}