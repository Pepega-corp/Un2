using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;

namespace Unicon2.Fragments.Journals.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RecordTemplate : IRecordTemplate
    {
        public RecordTemplate()
        {
            JournalParameters = new List<IJournalParameter>();
        }

        [JsonProperty]
        public List<IJournalParameter> JournalParameters { get; set; }
    }
}
