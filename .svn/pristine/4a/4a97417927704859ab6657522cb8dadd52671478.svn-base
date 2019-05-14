using System.Runtime.Serialization;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [DataContract(Namespace = "OscilloscopeTagNS")]
   public class OscilloscopeTag:IOscilloscopeTag
    {
        [DataMember]
        public IJournalParameter RelatedJournalParameter { get; set; }
        [DataMember]
        public string TagKey { get; set; }
    }
}
