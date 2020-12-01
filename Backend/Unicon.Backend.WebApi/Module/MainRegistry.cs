using Unicon.Backend.Common.Modularity;
using Unicon.Backend.WebApi.Handlers.Command;
using Unicon.Backend.WebApi.Handlers.Query;
using Unicon.Backend.WebApi.Interfaces;
using Unicon.Common.Commands;
using Unicon.Common.Queries;

namespace Unicon.Backend.WebApi.Module
{
    public class MainRegistry : IRegistry
    {
        public void Init(ITypesProvider typesProvider)
        {
            IHandlerRegistry handlerRegistry = typesProvider.Resolve<IHandlerRegistry>();
            handlerRegistry.RegisterHandler<GetDeviceDefinitionsQuery, GetDeviceDefinitionsQueryHandler>();
            handlerRegistry.RegisterHandler<UpdateDeviceDefinitionCommand, UpdateDeviceDefinitionCommandHandler>();
            handlerRegistry.RegisterHandler<GetStoreSnapshotQuery, GetStoreSnapshotQueryHandler>();
            handlerRegistry.RegisterHandler<UploadSnapshotCommand, UploadSnapshotCommandHandler>();

        }
    }
}