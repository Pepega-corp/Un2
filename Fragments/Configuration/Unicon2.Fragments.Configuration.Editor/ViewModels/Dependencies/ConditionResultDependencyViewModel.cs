using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Infrastructure.Extensions;
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
            var resultList = ResultViewModels.CloneCollection().ToList();
            var conditionsList = ConditionViewModels.CloneCollection().ToList();
            var actualResult = this.SelectedResultViewModel.Clone();
            var actualCondition =
                this.SelectedConditionViewModel?.Clone();
            resultList.ReplaceStronglyNamedInCollection(actualResult);
            conditionsList.ReplaceStronglyNamedInCollection(actualCondition);
            return new ConditionResultDependencyViewModel(resultList,
                conditionsList)
            {
                SelectedConditionViewModel = actualCondition,
                SelectedResultViewModel = actualResult
            };
        }

        public string Name => "ConditionResultDependency";
    }
}