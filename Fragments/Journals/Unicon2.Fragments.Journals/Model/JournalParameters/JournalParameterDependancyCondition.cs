using System.Collections;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JournalParameterDependancyCondition : IJournalCondition
    {
        [JsonProperty]
        public ConditionsEnum ConditionsEnum { get; set; }
        [JsonProperty]
        public ushort UshortValueToCompare { get; set; }
        [JsonProperty]
        public IUshortsFormatter UshortsFormatter { get; set; }
        [JsonProperty]
        public IJournalParameter BaseJournalParameter { get; set; }
        public string StrongName => nameof(JournalParameterDependancyCondition);

        [JsonProperty]
        public string Name { get; set; }

        public object Clone()
        {
            return new JournalParameterDependancyCondition();
        }
    }
}
