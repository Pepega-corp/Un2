using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IUshortFormattableEditorViewModel : INameable
    {
        IFormatterParametersViewModel FormatterParametersViewModel { get; set; }
    }
}