using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface IDynamicFormatterViewModel : IUshortsFormatterViewModel, IEditable
    {

        bool IsValid { get; }
    }
}