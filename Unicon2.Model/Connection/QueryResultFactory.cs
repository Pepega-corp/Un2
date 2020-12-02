using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;


namespace Unicon2.Model.Connection
{
    public class QueryResultFactory : IQueryResultFactory
    {
        public IQueryResult<T> CreateDefaultQueryResult<T>()
        {
            return new DefaultQueryResult<T>();
        }

        public IQueryResult CreateDefaultQueryResult()
        {
            return new DefaultQueryResult();
        }
    }
}