using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon.Backend.WebApi.Context;

namespace Unicon.Backend.WebApi.Interfaces
{
	public interface IHandlerTyped<TInput, TResult>
	{
		Task<TResult> HandleTyped(TInput input, ApplicationContext applicationContext);
	}

	public interface IHandler
	{
		Task<object> Handle(object input, ApplicationContext applicationContext);
	}
}
