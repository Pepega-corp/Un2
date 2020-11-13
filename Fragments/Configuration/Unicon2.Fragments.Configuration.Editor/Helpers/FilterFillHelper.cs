using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters;
using Unicon2.Fragments.Configuration.Model.Conditions;
using Unicon2.Fragments.Configuration.Model.Filter;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class FilterFillHelper
    {
        public IFilterViewModel CreateFilterViewModel(IFilter filter)
        {
            
        }

        public IFilter CreateFilter(IFilterViewModel filterViewModel)
        {
            switch (filterViewModel)
            {
                case FilterViewModel defaultFilterViewModel:
                    var res = new DefaultFilter();
                    var conditions = defaultFilterViewModel.ConditionViewModels.Select(model =>
                    {
                        var vm = (model as CompareConditionViewModel);
                        var condition = new CompareCondition();
                        if (Enum.TryParse<ConditionsEnum>(vm.SelectedCondition,
                            out var conditionsEnum))
                        {

                        }

                        return condition;
                    }).ToList();
                    res.Condition = conditions.Cast<ICondition>().ToList();
                    res.Name = filterViewModel.Name;
                    return res;
                    break;
            }
            return null;
        }
        
        p
    }
}