using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters;
using Unicon2.Fragments.Configuration.Model.Conditions;
using Unicon2.Fragments.Configuration.Model.Filter;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.Extensions;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class FilterFillHelper
    {
        private readonly ConditionFillHelper _conditionFillHelper;

        public FilterFillHelper(ConditionFillHelper conditionFillHelper)
        {
            _conditionFillHelper = conditionFillHelper;
        }

        public IFilterViewModel CreateFilterViewModel(IFilter filter)
        {
            switch (filter)
            {
                case DefaultFilter defaultFilter:
                    return new FilterViewModel(defaultFilter.Conditions
                        .Select(condition => _conditionFillHelper.CreateConditionViewModel(condition))
                        .ToObservableCollection())
                    {
                        Name = defaultFilter.Name
                    };
            }

            return null;
        }

        public IFilter CreateFilter(IFilterViewModel filterViewModel)
        {
            switch (filterViewModel)
            {
                case FilterViewModel defaultFilterViewModel:
                    var res = new DefaultFilter();
                    res.Conditions = defaultFilterViewModel.ConditionViewModels
                        .Select(model => _conditionFillHelper.CreateConditionFromViewModel(model))
                        .Where(condition => condition != null)
                        .ToList();
                    res.Name = filterViewModel.Name;
                    return res;
            }

            return null;
        }

    }
}