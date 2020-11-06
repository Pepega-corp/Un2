using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class FilterEditorWindowViewModel
    {
        public FilterEditorWindowViewModel(List<IFilterViewModel> filterViewModels)
        {
            FilterViewModels=new ObservableCollection<IFilterViewModel>(filterViewModels);
            AddFilterCommand=new RelayCommand(OnAddFilterExecute);
            DeleteFilterCommand=new RelayCommand<object>(OnDeleteFilterExecute);
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
