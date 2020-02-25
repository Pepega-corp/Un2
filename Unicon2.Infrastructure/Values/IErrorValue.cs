namespace Unicon2.Infrastructure.Values
{
    public interface IErrorValue : IFormattedValue
    {
        string ErrorMessage { get; set; }
    }
}