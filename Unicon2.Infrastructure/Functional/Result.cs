using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Infrastructure.Functional
{
    
    public class Result<T>
    {
        private Result(T item, bool isSuccess)
        {
            Item = item;
            IsSuccess = isSuccess;
        }

        private Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        private Result(Exception exception)
        {
            Exception = exception;
            IsSuccess = false;
        }


        public static Result<T> Create(T item, bool isSuccess)
        {
            return new Result<T>(item, isSuccess);
        }

        public static Result<T> Create(bool isSuccess)
        {
            return new Result<T>(isSuccess);
        }
        public static Result<T> CreateWithException(Exception exception)
        {
            return new Result<T>(exception);
        }
        public static Result<T> Create(Func<T> creator, bool isSuccess)
        {
            return isSuccess ? new Result<T>(creator(), true) : new Result<T>(false);
        }

        public static Result<T> Create(Func<T> creator, Func<bool> isSuccess)
        {
            return isSuccess() ? new Result<T>(creator(), true) : new Result<T>(false);
        }
        public static implicit operator Result<T>(T value)
        {
            if(value == null)
                return new Result<T>(false);
 
            return new Result<T>(value,true);
        }
        public Result<T> OnSuccess(Action<T> onSuccessFunc)
        {
            if (IsSuccess)
            {
                onSuccessFunc(Item);
            }

            return Item;
        }
        public Result OnSuccess(Func<T,Result> onSuccessFunc)
        {
            if (IsSuccess)
            {
                return onSuccessFunc(Item);
            }
            return Result.Create(false);
        }
        public Result<TTo> OnSuccess<TTo>(Func<T,Result<TTo>> onSuccessFunc)
        {
            if (IsSuccess)
            {
                return onSuccessFunc(Item);
            }
            return new Result<TTo>(false);
        }
        public async Task<Result<TTo>> OnSuccessAsync<TTo>(Func<T, Task<Result<TTo>>> onSuccessFunc)
        {
            if (IsSuccess)
            {
                return await onSuccessFunc(Item);
            }
            return new Result<TTo>(false);
        }

        public async Task<T> OnSuccessAsync(Func<T,Task<T>> onSuccessFunc)
        {
            if (IsSuccess)
            {
               return await onSuccessFunc(Item);
            }

            return Item;
        }
        public T OnFail(Action<T> onFailFunc)
        {
	        if (!IsSuccess)
	        {
		        onFailFunc(Item);
	        }

	        return Item;
        }
        public T Item { get; }

        public bool IsSuccess { get; }
        public Exception Exception { get; }

    }

    public class Result
    {
        public Exception Exception { get; }

        private Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        private Result(Exception exception)
        {
            Exception = exception;
            IsSuccess = false;
        }

        public bool IsSuccess { get; }

        public static Result Create(bool isSuccess)
        {
            return new Result(isSuccess);
        }
        public static Result Create(Exception exception)
        {
            return new Result(exception);
        }

        public static Result CreateMergeAnd(Result result1, Result result2)
        {
            return Create(result1.IsSuccess && result2.IsSuccess);
        }

        public static Result CreateMergeAnd(bool isSuccess, Result previousResult)
        {
            return Create(previousResult.IsSuccess && isSuccess);
        }
        public static implicit operator Result(bool isSuccess)
        {
            return new Result(isSuccess);
        }

   
    }

}