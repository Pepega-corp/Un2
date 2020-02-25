using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IStringValueViewModel : IFormattedValueViewModel<IStringValue>
    {
        string StringValue { get; set; }
    }
}