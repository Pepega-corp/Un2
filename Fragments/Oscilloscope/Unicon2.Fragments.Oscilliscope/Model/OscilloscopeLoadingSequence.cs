using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OscilloscopeJournalLoadingSequence : IOscilloscopeJournalLoadingSequence
    {
        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE;     

        [JsonProperty]
        public ushort AddressOfRecord { get; set; }
        [JsonProperty]
        public ushort NumberOfPointsInRecord { get; set; }
    }
}
