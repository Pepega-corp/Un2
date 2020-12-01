using System.Collections.ObjectModel;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Windows
{
    public interface IProjectBrowserViewModel : IAnchorableWindow
    {
        ObservableCollection<IDeviceViewModel> DeviceViewModels { get; set; }
    }
}