using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OscilloscopeTag : IOscilloscopeTag
    {
        [JsonProperty] public IJournalParameter RelatedJournalParameter { get; set; }
        [JsonProperty] public string TagKey { get; set; }
    }
}