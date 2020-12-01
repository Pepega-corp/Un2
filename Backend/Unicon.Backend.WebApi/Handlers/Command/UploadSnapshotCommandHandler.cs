using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Interfaces;
using Unicon.Common.Commands;
using Unicon.Common.Result;

namespace Unicon.Backend.WebApi.Handlers.Command
{
	public class UploadSnapshotCommandHandler : HandlerBase<UploadSnapshotCommand, bool>
	{
		private readonly IHandlerRegistry _handlerRegistry;

		public UploadSnapshotCommandHandler(IHandlerRegistry handlerRegistry)
		{
			_handlerRegistry = handlerRegistry;
		}
		public override async Task<bool> HandleTyped(UploadSnapshotCommand input,
			ApplicationContext applicationContext)
		{
			try
			{
				applicationContext.UpdateDeviceDefinitionCommands.RemoveRange(applicationContext
					.UpdateDeviceDefinitionCommands);
				await applicationContext.SaveChangesAsync();
				applicationContext.CommandsStoreEntries.RemoveRange(applicationContext.CommandsStoreEntries);
				await applicationContext.SaveChangesAsync();
				foreach (var commandRecord in input.CommandRecords)
				{
					if (commandRecord.CommandType == nameof(UpdateDeviceDefinitionCommand))
					{
						var payload = JsonConvert.DeserializeObject<UpdateDeviceDefinitionCommand>(commandRecord.CommandValue);
						await _handlerRegistry.ResolveHandler(payload
							.GetType()).Handle(payload, applicationContext);
					}
					
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
