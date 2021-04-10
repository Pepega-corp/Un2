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
        private ushort _currentRecordNumber = 0;
        private ushort[] _currentRecordValues;
        private IOscilloscopeLoadingSequenceInitializingParameters _oscilloscopeLoadingSequenceInitializingParameters;


        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE;

        public void Initialize(IJournalSequenceInitializingParameters journalSequenceInitializingParameters)
        {
            _oscilloscopeLoadingSequenceInitializingParameters =
                journalSequenceInitializingParameters as IOscilloscopeLoadingSequenceInitializingParameters;
        }

        [JsonProperty]
        public ushort AddressOfRecord { get; set; }
        [JsonProperty]
        public ushort NumberOfPointsInRecord { get; set; }
    }
}
