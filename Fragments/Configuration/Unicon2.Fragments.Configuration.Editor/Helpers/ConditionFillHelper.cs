using System;
using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Model.Conditions;
using Unicon2.Fragments.Configuration.Model.Dependencies.Conditions;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.Factories;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class ConditionFillHelper
    {
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;

        public ConditionFillHelper(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
        {
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
        }

        public List<IConditionViewModel> CreateEmptyAvailableConditionViewModels()
        {
            return new List<IConditionViewModel>()
            {
                new CompareResourceConditionViewModel(_sharedResourcesGlobalViewModel,
                    new List<string>(Enum.GetNames(typeof(ConditionsEnum)))),
                new RegexMatchConditionViewModel(_sharedResourcesGlobalViewModel)
            };
        }

        public IConditionViewModel CreateConditionViewModel(ICondition condition)
        {
            switch (condition)
            {
                case ICompareResourceCondition compareResourceCondition:
                    return new CompareResourceConditionViewModel(_sharedResourcesGlobalViewModel,
                        new List<string>(Enum.GetNames(typeof(ConditionsEnum))))
                    {
                        SelectedCondition = compareResourceCondition.ConditionsEnum.ToString(),
                        ReferencedResourcePropertyName = compareResourceCondition.ReferencedPropertyResourceName,
                        UshortValueToCompare = compareResourceCondition.UshortValueToCompare
                    };
                case ICompareCondition compareCondition:
                    return new CompareConditionViewModel(new List<string>(Enum.GetNames(typeof(ConditionsEnum))))
                    {
                        SelectedCondition = compareCondition.ConditionsEnum.ToString(),
                        UshortValueToCompare = compareCondition.UshortValueToCompare
                    };
                case IRegexMatchCondition regexMatchCondition:
                    return new RegexMatchConditionViewModel(_sharedResourcesGlobalViewModel)
                    {
                        ReferencedResourcePropertyName = regexMatchCondition.ReferencedPropertyResourceName,
                        RegexPattern = regexMatchCondition.RegexPattern
                    };
            }

            return null;
        }

        public ICondition CreateConditionFromViewModel(IConditionViewModel condition)
        {
            switch (condition)
            {
                case CompareResourceConditionViewModel compareResourceConditionViewModel:
                    if (Enum.TryParse<ConditionsEnum>(compareResourceConditionViewModel.SelectedCondition,
                        out var conditionsEnum))
                    {
                        return new CompareResourceCondition()
                        {
                            ConditionsEnum = conditionsEnum,
                            ReferencedPropertyResourceName =
                                compareResourceConditionViewModel.ReferencedResourcePropertyName,
                            UshortValueToCompare = compareResourceConditionViewModel.UshortValueToCompare
                        };
                    }
                    break;
                case CompareConditionViewModel compareConditionViewModel:
                    if (Enum.TryParse<ConditionsEnum>(compareConditionViewModel.SelectedCondition,
                        out var conditionsEnum1))
                    {
                        return new CompareCondition()
                        {
                            ConditionsEnum = conditionsEnum1,
                            UshortValueToCompare = compareConditionViewModel.UshortValueToCompare
                        };
                    }
                    break;
                case RegexMatchConditionViewModel regexMatchConditionViewModel:
                    return new RegexMatchCondition()
                    {
                        RegexPattern = regexMatchConditionViewModel.RegexPattern,
                        ReferencedPropertyResourceName = regexMatchConditionViewModel.ReferencedResourcePropertyName
                    };
                    break;
            }

            return null;
        }
    }
}