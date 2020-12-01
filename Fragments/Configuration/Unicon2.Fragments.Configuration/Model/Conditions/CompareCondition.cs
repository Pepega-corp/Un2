using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Model.Conditions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CompareCondition : ICompareCondition
    {
        [JsonProperty] public ConditionsEnum ConditionsEnum { get; set; }
        [JsonProperty] public ushort UshortValueToCompare { get; set; }
    }
}