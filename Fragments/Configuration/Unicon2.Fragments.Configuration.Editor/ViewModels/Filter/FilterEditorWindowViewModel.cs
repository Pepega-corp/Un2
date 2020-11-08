using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class FilterEditorWindowViewModel
    {
        private readonly IConfigurationGroupEditorViewModel _groupEditorViewModel;

        public FilterEditorWindowViewModel(IConfigurationGroupEditorViewModel groupEditorViewModel)
        {
            _groupEditorViewModel = groupEditorViewModel;
            FilterViewModels=new ObservableCollection<IFilterViewModel>(groupEditorViewModel.FilterViewModels.ToList());
            AddFilterCommand=new RelayCommand(OnAddFilterExecute);
            DeleteFilterCommand=new RelayCommand<object>(OnDeleteFilterExecute,CanExecuteDeleteFilter);
        }

        private bool CanExecuteDeleteFilter(object obj)
        {
            return obj != null;
        }

        private void OnDeleteFilterExecute(object obj)
        {
            if (obj is IFilterViewModel filterViewModel)
            {
                FilterViewModels.Remove(filterViewModel);
            }
        }

        private void OnAddFilterExecute()
        {
            FilterViewModels.Add(new FilterViewModel(new ObservableCollection<IConditionViewModel>()));
        }

        public ObservableCollection<IFilterViewModel> FilterViewModels { get; }
        public ICommand AddFilterCommand { get; }
        public ICommand DeleteFilterCommand { get; }

    }
}
