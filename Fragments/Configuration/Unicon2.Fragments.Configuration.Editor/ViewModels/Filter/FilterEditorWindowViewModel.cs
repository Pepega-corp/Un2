using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class FilterEditorWindowViewModel
    {
        private readonly IConfigurationGroupEditorViewModel _groupEditorViewModel;

        public FilterEditorWindowViewModel(IConfigurationGroupEditorViewModel groupEditorViewModel)
        {
            _groupEditorViewModel = groupEditorViewModel;
            FilterViewModels =
                groupEditorViewModel.FilterViewModels
                    .Select(model => model.Clone())
                    .ToObservableCollection();
            AddFilterCommand = new RelayCommand(OnAddFilterExecute);
            DeleteFilterCommand = new RelayCommand<object>(OnDeleteFilterExecute);
            SubmitCommand = new RelayCommand<object>(this.OnSubmitExecute);
            CancelCommand = new RelayCommand<object>(this.OnCancelExecute);
        }

        private void CloseWindow(object window)
        {
            (window as Window)?.Close();
        }
        private void OnCancelExecute(object obj)
        {
            this.CloseWindow(obj);
        }

        private void OnSubmitExecute(object obj)
        {
            _groupEditorViewModel.FilterViewModels.Clear();
            FilterViewModels.ForEach(model => _groupEditorViewModel.FilterViewModels.Add(model));
            this.CloseWindow(obj);
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
            FilterViewModels.Add(new FilterViewModel(new ObservableCollection<IConditionViewModel>())
            {
                Name = "Filter" + (FilterViewModels.Count + 1)
            });
        }

        public ObservableCollection<IFilterViewModel> FilterViewModels { get; }
        public ICommand AddFilterCommand { get; }
        public ICommand DeleteFilterCommand { get; }

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }
    }
}
