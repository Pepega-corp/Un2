using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters;

namespace Unicon2.Fragments.Configuration.Model.Filter
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GroupFilterInfo : IGroupFilterInfo
    {
        [JsonProperty] public List<IFilter> Filters { get; set; }
    }
}