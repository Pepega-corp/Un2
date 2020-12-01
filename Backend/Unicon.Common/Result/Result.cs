namespace Unicon.Common.Result
{
	public class Result<T>
	{
		public Result()
		{

		}
		public static Result<T> FailResult()
		{
			return new Result<T>(false);
		}

		public static Result<T> SuccessResult(T item)
		{
			return new Result<T>(true, item);
		}

		private Result(bool isSuccess = false)
		{
			IsSuccess = isSuccess;
		}

		private Result(bool isSuccess, T item)
		{
			IsSuccess = isSuccess;
			Item = item;
		}

		public bool IsSuccess { get; set; }
		public T Item { get; set; }
	}
}
