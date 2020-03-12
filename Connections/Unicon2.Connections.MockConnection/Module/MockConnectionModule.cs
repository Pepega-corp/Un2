using Unicon2.Connections.MockConnection.Factories;
using Unicon2.Connections.MockConnection.Keys;
using Unicon2.Connections.MockConnection.ViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.MockConnection.Module
{
    public class MockConnectionModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IDeviceConnection, Model.MockConnection>(StringKeys.MOCK_CONNECTION);
            container.Register<IDeviceConnectionViewModel, MockConnectionViewModel>(StringKeys.MOCK_CONNECTION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<Model.MockConnection>();
            //регистрация фабрики 
            container.Register<IDeviceConnectionFactory, MockConnectionFactory>(StringKeys.MOCK_CONNECTION_FACTORY_NAME);
            //регистрация ресурсов
            IXamlResourcesService xamlResourcesService = container.Resolve<IXamlResourcesService>();
            xamlResourcesService.AddResourceAsGlobal("Resources/MockConnectionResources.xaml", this.GetType().Assembly);
        }
    }
}
