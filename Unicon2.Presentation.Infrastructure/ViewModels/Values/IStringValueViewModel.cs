namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IStringValueViewModel:IFormattedValueViewModel
    {
        string StringValue { get; set; }
    }
}