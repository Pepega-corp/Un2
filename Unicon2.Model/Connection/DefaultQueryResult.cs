using System;
using Unicon2.Infrastructure.Connection;

namespace Unicon2.Model.Connection
{
    public class DefaultQueryResult<T> : DefaultQueryResult, IQueryResult<T>
    {
        public T Result { get; set; }
    }

    public class DefaultQueryResult : IQueryResult
    {
        public bool IsSuccessful { get; set; }
        public void AddExeption(Exception e)
        {
            this.Exception = e;
        }

        public Exception Exception { get; private set; }
    }


}
