using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DependentJournalParameter : JournalParameter, IDependentJournalParameter
    {
        public DependentJournalParameter()
        {
            JournalParameterDependancyConditions = new List<IJournalCondition>();
        }

        [JsonProperty]
        public List<IJournalCondition> JournalParameterDependancyConditions { get; set; }
      
        
    }
}
