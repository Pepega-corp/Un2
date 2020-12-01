using System.Collections.Generic;

namespace Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters
{
    public interface ISubJournalParameter : IJournalParameter
    {
        List<int> BitNumbersInWord { get; set; }
    }
}