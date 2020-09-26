using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon.Backend.Common.Modularity;
using Unicon.Backend.WebApi.Handlers.Command;
using Unicon.Backend.WebApi.Handlers.Query;
using Unicon.Backend.WebApi.Interfaces;
using Unicon.Common.Queries;

namespace Unicon.Backend.WebApi.Module
{
	public class MainModule : IModule
	{
		public void InitializeTypes(ITypesContainer typesContainer)
		{
			typesContainer.Register<GetDeviceDefinitionsQueryHandler>();
			typesContainer.Register<UpdateDeviceDefinitionCommandHandler>();
			typesContainer.Register<GetStoreSnapshotQueryHandler>();
			typesContainer.Register<UploadSnapshotCommandHandler>();

			typesContainer.Register<IHandlerRegistry, HandlerRegistry>(true);
		}
	}
}
