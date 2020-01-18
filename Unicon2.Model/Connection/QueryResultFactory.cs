using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;


namespace Unicon2.Model.Connection
{
  public  class QueryResultFactory: IQueryResultFactory
    {
        public IQueryResult<T> CreateDefaultQueryResult<T>()
        {
          return StaticContainer.Container.Resolve(typeof(IQueryResult<T>)) as IQueryResult<T>;
        }

        public IQueryResult CreateDefaultQueryResult()
        {
            return StaticContainer.Container.Resolve(typeof(IQueryResult)) as IQueryResult;
        }
    }
}
