using Unicon2.DeviceEditorUtilityModule.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.Views
{
    /// <summary>
    /// Interaction logic for DeviceEditorView.xaml
    /// </summary>
    public partial class DeviceEditorView 
    {
        public DeviceEditorView(DeviceEditorViewModel deviceEditorViewModel)
        {
            InitializeComponent();
            DataContext = deviceEditorViewModel;
        }
        
    }
}
