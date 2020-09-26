using System;
using System.Collections.Generic;
using System.Text;

namespace Unicon.Backend.Common.Modularity
{
	public interface ITypesProvider
	{
		T Resolve<T>();
		object Resolve(Type type);

	}

	public interface ITypesContainer
	{
		void Register<T>(bool isSingleton = false) where T : class;

		void Register<TFrom, TTo>(bool isSingleton = false) where TTo : class, TFrom where TFrom : class;
	}

}
