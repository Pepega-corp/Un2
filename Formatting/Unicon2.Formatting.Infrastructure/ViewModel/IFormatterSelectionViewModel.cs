using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface IFormatterSelectionViewModel
    {
        ObservableCollection<IUshortsFormatterViewModel> UshortsFormatterViewModels { get; set; }
        IUshortsFormatterViewModel SelectedUshortsFormatterViewModel { get; set; }
        string FormatterOwnersName { get; }
        ICommand ResetCommand { get; }

        //ICommand SetInBufferCommand { get;  }
        //ICommand GetFromBufferCommand { get;  }
        ICommand AddAsResourceCommand { get; }
        ICommand SelectFromResourcesCommand { get; }
        string CurrentResourceString { get; set; }
        bool IsFormatterFromResource { get; set; }
    }
}