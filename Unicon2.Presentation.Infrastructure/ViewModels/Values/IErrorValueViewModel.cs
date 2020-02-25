using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IErrorValueViewModel : IFormattedValueViewModel<IErrorValue>
    {
        string ErrorMessage { get; set; }
    }
}