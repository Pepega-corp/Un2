using System;

namespace Unicon.Common.Interfaces
{
	public abstract class Query<TResult>: IQuery
	{
		private Type Result => typeof(TResult);
	}
}