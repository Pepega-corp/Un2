using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Journals.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JournalRecord : IJournalRecord
    {
        public JournalRecord()
        {
            FormattedValues = new List<IFormattedValue>();
        }

        [JsonProperty] public int NumberOfRecord { get; set; }
        //TODO: посмотреть сериализацию записей журнала, пишет какую-то херню,когда записывает этот лист
        [JsonProperty] public List<IFormattedValue> FormattedValues { get; set; }
    }
}