using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies
{
    public class ConditionResultDependencyViewModel : ViewModelBase, IDependencyViewModel
    {
        private IResultViewModel _selectedResultViewModel;
        private IConditionViewModel _selectedConditionViewModel;

        public ConditionResultDependencyViewModel(List<IResultViewModel> resultViewModels,
            List<IConditionViewModel> conditionViewModels)
        {
            ResultViewModels = resultViewModels;
            ConditionViewModels = conditionViewModels;
        }

        public List<IResultViewModel> ResultViewModels { get; }
        public List<IConditionViewModel> ConditionViewModels { get; }

        public IConditionViewModel SelectedConditionViewModel
        {
            get => _selectedConditionViewModel;
            set
            {
                _selectedConditionViewModel = value;
                RaisePropertyChanged();
            }
        }

        public IResultViewModel SelectedResultViewModel
        {
            get => _selectedResultViewModel;
            set
            {
                _selectedResultViewModel = value;
                RaisePropertyChanged();
            }
        }

        public IDependencyViewModel Clone()
        {
            return new ConditionResultDependencyViewModel(ResultViewModels.CloneCollection().ToList(), ConditionViewModels.CloneCollection().ToList())
            {
                SelectedConditionViewModel = this.SelectedConditionViewModel.Clone(),
                SelectedResultViewModel = SelectedResultViewModel.Clone()
            };
        }

        public string Name { get; }
    }
}