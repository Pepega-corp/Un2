using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.OfflineConnection
{
    /// <summary>
    /// класс модульной интеграции в приложение
    /// </summary>
    public class OfflineConnectionModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            //регистрация фабрики 
            container.Register<IDeviceConnectionFactory, OfflineConnectionFactory>(ApplicationGlobalNames.OFFLINE_CONNECTION_FACTORY_NAME);
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/OfflineConnectionResources.xaml",this.GetType().Assembly);

            container.Resolve<ISerializerService>().AddKnownTypeForSerialization(typeof(OfflineConnection));
        }
    }
}
