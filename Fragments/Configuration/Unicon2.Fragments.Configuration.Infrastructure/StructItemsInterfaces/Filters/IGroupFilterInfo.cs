using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters
{
    public interface IGroupFilterInfo
    {
        List<IFilter> Filters { get; set; }
    }
}