using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IErrorValueViewModel : IFormattedValueViewModel
    {
        string ErrorMessage { get; set; }
    }
}