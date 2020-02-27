using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IStringValueViewModel : IFormattedValueViewModel
    {
        string StringValue { get; set; }
    }
}