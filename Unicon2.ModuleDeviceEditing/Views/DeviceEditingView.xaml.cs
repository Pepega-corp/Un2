using Unicon2.ModuleDeviceEditing.ViewModels;

namespace Unicon2.ModuleDeviceEditing.Views
{
    /// <summary>
    /// Interaction logic for DeviceEditingView.xaml
    /// </summary>
    public partial class DeviceEditingView 
    {
        public DeviceEditingView(DeviceEditingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}