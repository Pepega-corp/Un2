using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters
{
    public interface IFilter
    {
        string Name { get; set; }
        List<ICondition> Conditions { get; set; }
    }
}