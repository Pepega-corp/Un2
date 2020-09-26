using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon.Backend.Common.Modularity;

namespace Unicon.Backend.WebApi.Interfaces
{
	public class HandlerRegistry : IHandlerRegistry
	{
		private readonly ITypesProvider _typesProvider;
		private Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();

		public HandlerRegistry(ITypesProvider typesProvider)
		{
			_typesProvider = typesProvider;
		}


		public void RegisterHandler<TInput, THandler>() where THandler : IHandler
		{
			_handlers.Add(typeof(TInput), typeof(THandler));
		}

		public IHandler ResolveHandler(Type inputType)
		{
			return _typesProvider.Resolve(_handlers[inputType]) as IHandler;
		}

	}
}