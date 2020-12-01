using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [JsonObject(MemberSerialization.OptIn)]

    public class OptionPossibleValue : IOptionPossibleValue
    {
        public OptionPossibleValue()
        {
            PossibleValueConditions = new List<IPossibleValueCondition>();
        }

        [JsonProperty] public string PossibleValueName { get; set; }
        [JsonProperty] public List<IPossibleValueCondition> PossibleValueConditions { get; set; }
    }
}