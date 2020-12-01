using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Unicon.Backend.Common.Modularity;

namespace Unicon.Backend.WebApi.Module
{
	public class TypesProvider : ITypesProvider
	{
		private readonly IServiceProvider _serviceProvider;

		public TypesProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public T Resolve<T>()
		{
			return _serviceProvider.GetService<T>();
		}

		public object Resolve(Type type)
		{
			return _serviceProvider.GetService(type);
		}


	}


	public class TypesContainer : ITypesContainer
	{
		private readonly IServiceCollection _serviceCollection;

		public TypesContainer(IServiceCollection serviceCollection)
		{
			_serviceCollection = serviceCollection;
		}

		public void Register<T>(bool isSingleton = false) where T : class
		{
			if (isSingleton)
			{
				_serviceCollection.AddSingleton<T>();
			}
			else
			{
				_serviceCollection.AddTransient<T>();
			}
		}


		public void Register<TFrom, TTo>(bool isSingleton = false) where TTo : class, TFrom where TFrom : class
		{
			if (isSingleton)
			{
				_serviceCollection.AddSingleton<TFrom, TTo>();
			}
			else
			{
				_serviceCollection.AddTransient<TFrom, TTo>();
			}
		}
	}
}
