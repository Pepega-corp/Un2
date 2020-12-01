using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon.Backend.WebApi.Context;
using Unicon.Backend.WebApi.Interfaces;

namespace Unicon.Backend.WebApi.Handlers
{
	public abstract class HandlerBase<TInput, TResult> : IHandler, IHandlerTyped<TInput, TResult>
	{
		public async Task<object> Handle(object input ,ApplicationContext applicationContext)
		{
			return await HandleTyped((TInput)input,  applicationContext);
		}

		public abstract Task<TResult> HandleTyped(TInput input, ApplicationContext applicationContext);
	}
}
