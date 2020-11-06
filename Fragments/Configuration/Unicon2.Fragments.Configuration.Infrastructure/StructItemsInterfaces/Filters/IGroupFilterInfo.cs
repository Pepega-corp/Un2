using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters
{
    public interface IGroupFilterInfo
    {
        List<IFilter> Filters { get; set; }
    }
}