using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SubJournalParameter : JournalParameter, ISubJournalParameter
    {
        public SubJournalParameter()
        {
            BitNumbersInWord = new List<int>();
        }

        [JsonProperty]
        public List<int> BitNumbersInWord { get; set; }
    }
}
