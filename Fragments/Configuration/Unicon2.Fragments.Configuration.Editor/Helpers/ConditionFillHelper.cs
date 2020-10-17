using System;
using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
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
                new CompareResourceConditionViewModel(_sharedResourcesGlobalViewModel, new List<string>(Enum.GetNames(typeof(ConditionsEnum))))
            };
        }

        public IConditionViewModel CreateConditionViewModel(ICondition condition)
        {
            switch (condition)
            {
                case ICompareResourceCondition compareResourceCondition:
                    return new CompareResourceConditionViewModel(_sharedResourcesGlobalViewModel, new List<string>(Enum.GetNames(typeof(ConditionsEnum))))
                    {
                        SelectedCondition = compareResourceCondition.ConditionsEnum.ToString(),
                        ReferencedResourcePropertyName = compareResourceCondition.ReferencedPropertyResourceName,
                        UshortValueToCompare = compareResourceCondition.UshortValueToCompare
                    };
                    break;
            }
            return null;
        }
        
        public ICondition CreateConditionFromViewModel(IConditionViewModel condition)
        {
            switch (condition)
            {
                case CompareResourceConditionViewModel compareResourceConditionViewModel:
                    if (Enum.TryParse<ConditionsEnum>(compareResourceConditionViewModel.SelectedCondition, out var conditionsEnum))
                    {
                        return new CompareResourceCondition()
                        {
                            ConditionsEnum = conditionsEnum,
                            ReferencedPropertyResourceName = compareResourceConditionViewModel.ReferencedResourcePropertyName,
                            UshortValueToCompare = compareResourceConditionViewModel.UshortValueToCompare
                        };
                    }
                    break;
                
            }
            return null;
        }
    }
}