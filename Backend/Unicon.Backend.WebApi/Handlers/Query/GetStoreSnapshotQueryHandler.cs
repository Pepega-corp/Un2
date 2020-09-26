using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Entities;
using Unicon.Common.Model;
using Unicon.Common.Queries;
using Unicon.Common.Result;

namespace Unicon.Backend.WebApi.Handlers.Query
{
	public class GetStoreSnapshotQueryHandler : HandlerBase<GetStoreSnapshotQuery, Result<List<CommandRecord>>>
	{
		public override async Task<Result<List<CommandRecord>>> HandleTyped(GetStoreSnapshotQuery input,
			ApplicationContext applicationContext)
		{
			return Result<List<CommandRecord>>.SuccessResult(await applicationContext.CommandsStoreEntries
				.Where(entry => entry.CommandType == nameof(UpdateDeviceDefinitionCommand))
				.Join(applicationContext.UpdateDeviceDefinitionCommands, entry => entry.Id, command => command.Id,
					(entry, command) => new CommandRecord()
					{
						CommandType = entry.CommandType,
						CommandValue = JsonConvert.SerializeObject(command,new JsonSerializerSettings()
						{
							TypeNameHandling = TypeNameHandling.All
						}),
						SequenceNumber = entry.SequenceNumber
					}).ToListAsync());
		}
	}
}
