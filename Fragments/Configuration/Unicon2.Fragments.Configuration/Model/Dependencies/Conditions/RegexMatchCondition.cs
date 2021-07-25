using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;

namespace Unicon2.Fragments.Configuration.Model.Dependencies.Conditions
{
    [JsonObject(MemberSerialization.OptIn)]

    public class RegexMatchCondition : IRegexMatchCondition
    {
        [JsonProperty] public string RegexPattern { get; set; }
        [JsonProperty] public string ReferencedPropertyResourceName { get; set; }
    }
}