using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.FragmentSettings
{
    public interface IQuickAccessMemorySettingViewModel:IFragmentSettingViewModel
    {
        ObservableCollection<IRangeViewModel> RangeViewModels { get; set; }
        IRangeViewModel SelectedRangeViewModel { get; set; }
        ICommand AddRangeCommand { get; set; }
        ICommand DeleteRangeCommand { get; set; }
    }
}