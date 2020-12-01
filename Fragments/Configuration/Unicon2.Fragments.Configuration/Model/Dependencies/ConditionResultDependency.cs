using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;

namespace Unicon2.Fragments.Configuration.Model.Dependencies
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConditionResultDependency : IConditionResultDependency
    {
        [JsonProperty] public ICondition Condition { get; set; }
        [JsonProperty] public IDependencyResult Result { get; set; }
    }
}