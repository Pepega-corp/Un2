using System.Collections.Generic;

namespace Unicon2.Fragments.Journals.Infrastructure.Model
{
    public interface IRecordTemplate
    {
     
        List<IJournalParameter> JournalParameters { get; set; }
    }
}