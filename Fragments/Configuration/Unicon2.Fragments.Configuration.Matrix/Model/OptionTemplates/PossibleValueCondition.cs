using Newtonsoft.Json;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [JsonObject(MemberSerialization.OptIn)]

    public class PossibleValueCondition : IPossibleValueCondition
    {
        [JsonProperty]
        public bool BoolConditionRule { get; set; }
        [JsonProperty]
        public IOptionPossibleValue RelatedOptionPossibleValue { get; set; }
    }
}
