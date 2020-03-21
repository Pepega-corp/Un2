using System.Collections.Generic;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Journals.Infrastructure.Model
{
    public interface IJournalRecord 
    {
        int NumberOfRecord { get; set; }
        List<IFormattedValue> FormattedValues { get; set; }
    }
}