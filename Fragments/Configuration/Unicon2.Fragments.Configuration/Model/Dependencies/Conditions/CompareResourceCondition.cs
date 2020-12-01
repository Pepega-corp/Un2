using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Model.Dependencies.Conditions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CompareResourceCondition : ICompareResourceCondition
    {
        [JsonProperty] public string ReferencedPropertyResourceName { get; set; }
        [JsonProperty] public ushort UshortValueToCompare { get; set; }
        [JsonProperty] public ConditionsEnum ConditionsEnum { get; set; }
    }
}