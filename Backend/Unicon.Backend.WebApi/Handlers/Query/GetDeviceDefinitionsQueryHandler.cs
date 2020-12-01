using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Interfaces;
using Unicon.Common.Model;
using Unicon.Common.Queries;
using Unicon.Common.Result;

namespace Unicon.Backend.WebApi.Handlers.Query
{
	public class
		GetDeviceDefinitionsQueryHandler : HandlerBase<GetDeviceDefinitionsQuery, Result<List<DeviceDefinition>>>
	{

	

		public override async Task<Result<List<DeviceDefinition>>> HandleTyped(GetDeviceDefinitionsQuery input,
			ApplicationContext applicationContext)
		{
			if (input.Ids != null)
			{

			}

			return Result<List<DeviceDefinition>>.SuccessResult(await applicationContext.DeviceDefinitions
				.Include(definition => definition.Tags).Select(definition => new DeviceDefinition()
				{
					Id = definition.Id,
					DefinitionString = definition.DefinitionString,
					Tags = definition.Tags.Select(tag => tag.Name).ToList()
				})
				.ToListAsync());
		}

	}
}
