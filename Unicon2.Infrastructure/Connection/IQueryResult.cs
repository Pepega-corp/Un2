﻿using System;

namespace Unicon2.Infrastructure.Connection
{
    public interface IQueryResult<T> : IQueryResult
    {
        T Result { get; set; }
    }

    public interface IQueryResult
    {
        bool IsSuccessful { get; set; }
        void AddExeption(Exception e);
        Exception Exception { get; }
    }
}