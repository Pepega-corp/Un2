namespace Unicon2.Infrastructure.Functional
{
	public interface IResult
    {
        bool HasValue { get; }
        object Value { get; }
    }
}