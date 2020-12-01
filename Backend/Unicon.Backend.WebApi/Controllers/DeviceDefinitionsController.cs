using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Entities;
using Unicon.Backend.WebApi.Interfaces;
using Unicon.Common.Constants;
using Unicon.Common.Queries;
using Unicon.Common.Result;

namespace Unicon.Backend.WebApi.Controllers
{
	[ApiController]
	[Route(ApiConstants.ApiV1Path)]
	public class RootController : ControllerBase
	{
		private readonly ApplicationContext _context;
		private readonly IHandlerRegistry _handlerRegistry;

		public RootController(ApplicationContext context, IHandlerRegistry handlerRegistry)
		{
			_context = context;
			_handlerRegistry = handlerRegistry;
		}

		[Route(ApiConstants.ApiCommandRoute)]
		[HttpPost]
		public Task<object> Command([FromBody] object payload)
		{
			try
			{
				return _handlerRegistry.ResolveHandler(payload.GetType()).Handle(payload,_context);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

		}

		[Route(ApiConstants.ApiQueryRoute)]
		[HttpPost]
		public Task<object> Query([FromBody] object payload)
		{
			try
			{
				return _handlerRegistry.ResolveHandler(payload.GetType()).Handle(payload,_context);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

		}
	}
}