using Unicon2.Connections.MockConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.MockConnection.ViewModels
{
    public class MockConnectionViewModel : ViewModelBase, IDeviceConnectionViewModel
    {
        public string StrongName =>
            StringKeys.MOCK_CONNECTION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model { get; set; }

        public string ConnectionName => StringKeys.MOCK_CONNECTION;
    }
}