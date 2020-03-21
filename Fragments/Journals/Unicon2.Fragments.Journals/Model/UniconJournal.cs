using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Journals.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UniconJournal : IUniconJournal
    {
        public UniconJournal(IRecordTemplate recordTemplate)
        {
            RecordTemplate = recordTemplate;
            JournalRecords = new List<IJournalRecord>();
        }
        [JsonProperty] public IRecordTemplate RecordTemplate { get; set; }
        [JsonProperty] public IJournalLoadingSequence JournalLoadingSequence { get; set; }
        [JsonProperty] public List<IJournalRecord> JournalRecords { get; set; }
        public string StrongName => JournalKeys.UNICON_JOURNAL;
        public IFragmentSettings FragmentSettings { get; set; }
        [JsonProperty] public string Name { get; set; }
    }
}