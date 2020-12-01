using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters;

namespace Unicon2.Fragments.Configuration.Model.Filter
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultFilter : IFilter
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public List<ICondition> Conditions { get; set; }
    }
}