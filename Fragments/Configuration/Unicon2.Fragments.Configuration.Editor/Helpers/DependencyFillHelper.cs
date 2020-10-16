using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Model.Dependencies;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.Common;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class DependencyFillHelper
    {
        private readonly TypesContainer _typesContainer;
        private readonly ResultFillHelper _resultFillHelper;
        private readonly ConditionFillHelper _conditionFillHelper;

        public DependencyFillHelper(TypesContainer typesContainer,ResultFillHelper resultFillHelper,ConditionFillHelper conditionFillHelper)
        {
            _typesContainer = typesContainer;
            _resultFillHelper = resultFillHelper;
            _conditionFillHelper = conditionFillHelper;
        }

        public IDependencyViewModel CreateEmptyConditionResultDependencyViewModel()
        {
            var resultList = _typesContainer.ResolveAll<IResultViewModel>().ToList();
            var conditionsList = _typesContainer.ResolveAll<IConditionViewModel>().ToList();
            return new ConditionResultDependencyViewModel(resultList,
                conditionsList);
        }

        public IDependencyViewModel CreateDependencyViewModel(IDependency dependency)
        {
            switch (dependency)
            {
                case IConditionResultDependency conditionResultDependency:

                    var resultList = _typesContainer.ResolveAll<IResultViewModel>().ToList();
                    var conditionsList = _typesContainer.ResolveAll<IConditionViewModel>().ToList();
                    var actualResult = _resultFillHelper.CreateResultViewModel(conditionResultDependency.Result);
                    var actualCondition =
                        _conditionFillHelper.CreateConditionViewModel(conditionResultDependency.Condition);
                    resultList.ReplaceStronglyNamedInCollection(actualResult);
                    conditionsList.ReplaceStronglyNamedInCollection(actualCondition);
                    return new ConditionResultDependencyViewModel(resultList,
                        conditionsList)
                    {
                        SelectedConditionViewModel = actualCondition,
                        SelectedResultViewModel = actualResult
                    };
                    break;
                default:

                    break;
            }

            return null;
        }



        public IDependency CreateDependencyModel(IDependencyViewModel dependency)
        {
            switch (dependency)
            {
                case ConditionResultDependencyViewModel conditionResultDependencyViewModel:
                    return new ConditionResultDependency()
                    {
                        Condition = _conditionFillHelper.CreateConditionFromViewModel(conditionResultDependencyViewModel.SelectedConditionViewModel),
                        Result = _resultFillHelper.CreateResultFromViewModel(conditionResultDependencyViewModel.SelectedResultViewModel)
                    };
                    break;
            }
            return null;

        }
    }
}