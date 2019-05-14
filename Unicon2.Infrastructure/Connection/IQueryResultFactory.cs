namespace Unicon2.Infrastructure.Connection
{
    public interface IQueryResultFactory
    {
        IQueryResult<T> CreateDefaultQueryResult<T>();
        IQueryResult CreateDefaultQueryResult();
    }
}