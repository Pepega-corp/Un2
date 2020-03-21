using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ComplexJournalParameter : JournalParameter, IComplexJournalParameter
    {
        public ComplexJournalParameter()
        {
            ChildJournalParameters = new List<ISubJournalParameter>();
        }

        [JsonProperty]
        public List<ISubJournalParameter> ChildJournalParameters { get; set; }
    }
}
