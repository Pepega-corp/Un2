using System.Collections.ObjectModel;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Enums;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;

namespace Unicon2.Presentation.ViewModels.Windows
{
  public class ProjectBrowserViewModel:AnchorableWindowBase,IProjectBrowserViewModel
    {
        public ProjectBrowserViewModel()
        {
            WindowNameKey = ApplicationGlobalNames.WindowsStrings.PROJECT_STRING_KEY;
            AnchorableDefaultPlacementEnum=PlacementEnum.Left;
            DeviceViewModels = new ObservableCollection<IDeviceViewModel>();
        }


        private ObservableCollection<IDeviceViewModel> _deviceViewModels;

        #region Implementation of IProjectBrowserViewModel

        public ObservableCollection<IDeviceViewModel> DeviceViewModels
        {
            get => _deviceViewModels;
            set
            {
                _deviceViewModels = value; 
                RaisePropertyChanged();
            }
        }

        #endregion




     

    }
}
