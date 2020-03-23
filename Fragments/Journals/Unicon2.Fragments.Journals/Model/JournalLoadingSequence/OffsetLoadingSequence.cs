using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;

namespace Unicon2.Fragments.Journals.Model.JournalLoadingSequence
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OffsetLoadingSequence : IOffsetLoadingSequence
    {
        [JsonProperty]
        public int NumberOfRecords { get; set; }

        [JsonProperty]
        public ushort JournalStartAddress { get; set; }

        [JsonProperty]
        public ushort NumberOfPointsInRecord { get; set; }

        [JsonProperty]
        public ushort WordFormatFrom { get; set; }

        [JsonProperty]
        public ushort WordFormatTo { get; set; }

        [JsonProperty]
        public bool IsWordFormatNotForTheWholeRecord { get; set; }

        public string StrongName => JournalKeys.OFFSET_LOADING_SEQUENCE;
    }
}
