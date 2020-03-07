using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IUshortFormattableEditorViewModel : INameable
    {
        string SelectedUshortFormatterName { get; }
        ICommand ShowFormatterParameters { get; }
        IUshortsFormatterViewModel RelatedUshortsFormatterViewModel { get; set; }
    }
}