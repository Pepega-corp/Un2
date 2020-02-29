using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IUshortFormattableEditorViewModel
    {
        string SelectedUshortFormatterName { get; }
        ICommand ShowFormatterParameters { get; set; }
        IUshortsFormatterViewModel RelatedUshortsFormatterViewModel { get; set; }
    }
}