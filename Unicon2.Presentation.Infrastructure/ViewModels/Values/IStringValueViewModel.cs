using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IStringValueViewModel : IEditableValueViewModel
    {
        string StringValue { get; set; }
    }
}