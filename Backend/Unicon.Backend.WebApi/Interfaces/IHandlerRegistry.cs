using System;

namespace Unicon.Backend.WebApi.Interfaces
{
	public interface IHandlerRegistry
	{
		void RegisterHandler<TInput,THandler>() where THandler : IHandler;

		IHandler ResolveHandler(Type inputType);
	}
}