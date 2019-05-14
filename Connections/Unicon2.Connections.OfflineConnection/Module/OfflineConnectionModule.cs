using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.OfflineConnection.Module
{
    public class OfflineConnectionModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
           container.Resolve<ISerializerService>().AddKnownTypeForSerialization(typeof(OfflineConnection));
        }
    }
}
