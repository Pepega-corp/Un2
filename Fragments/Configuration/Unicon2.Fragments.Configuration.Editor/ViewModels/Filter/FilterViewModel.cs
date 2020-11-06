using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class FilterViewModel:ViewModelBase, IFilterViewModel
    {

        public FilterViewModel(ObservableCollection<IConditionViewModel> selectedConditionViewModels)
        {
            SelectedConditionViewModels = selectedConditionViewModels;
        }
        public string Name { get; set; }
        public ObservableCollection<IConditionViewModel> SelectedConditionViewModels { get; }

    }

}