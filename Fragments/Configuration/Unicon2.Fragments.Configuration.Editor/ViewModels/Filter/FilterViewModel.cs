using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class FilterViewModel:ViewModelBase, IFilterViewModel
    {
        private string _name;

        public FilterViewModel(ObservableCollection<IConditionViewModel> selectedConditionViewModels)
        {
            AddConditionCommand = new RelayCommand(OnAddConditionExecute);
            DeleteConditionCommand = new RelayCommand<object>(OnDeleteFilterExecute);
            ConditionViewModels = selectedConditionViewModels;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        private void OnDeleteFilterExecute(object obj)
        {
            if (obj is IConditionViewModel conditionViewModel)
            {
                ConditionViewModels.Remove(conditionViewModel);
            }
        }

        private void OnAddConditionExecute()
        {
            ConditionViewModels.Add(new CompareConditionViewModel(new List<string>(Enum.GetNames(typeof(ConditionsEnum)))));
        }

        public ObservableCollection<IConditionViewModel> ConditionViewModels { get; }
        public ICommand AddConditionCommand { get; }
        public ICommand DeleteConditionCommand { get; }

        public IFilterViewModel Clone()
        {
            return new FilterViewModel(ConditionViewModels.Select(model =>model.Clone() ).ToObservableCollection())
            {
                Name = Name
            };
        }
    }

}