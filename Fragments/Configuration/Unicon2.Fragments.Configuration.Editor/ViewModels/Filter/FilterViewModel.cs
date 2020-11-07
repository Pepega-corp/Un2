using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class FilterViewModel:ViewModelBase, IFilterViewModel
    {

        public FilterViewModel(ObservableCollection<IConditionViewModel> selectedConditionViewModels)
        {
            AddConditionCommand = new RelayCommand(OnAddFilterExecute);
            DeleteConditionCommand = new RelayCommand<object>(OnDeleteFilterExecute);
            ConditionViewModels = selectedConditionViewModels;
        }
        public string Name { get; set; }

        private void OnDeleteFilterExecute(object obj)
        {
            if (obj is IConditionViewModel conditionViewModel)
            {
                ConditionViewModels.Remove(conditionViewModel);
            }
        }

        private void OnAddFilterExecute()
        {
            ConditionViewModels.Add(new CompareConditionViewModel(new List<string>(Enum.GetNames(typeof(ConditionsEnum)))));
        }

        public ObservableCollection<IConditionViewModel> ConditionViewModels { get; }
        public ICommand AddConditionCommand { get; }
        public ICommand DeleteConditionCommand { get; }

    }

}