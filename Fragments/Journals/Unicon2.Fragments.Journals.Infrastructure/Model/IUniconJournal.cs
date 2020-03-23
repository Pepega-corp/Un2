using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Infrastructure.Model
{
    public interface IUniconJournal: IDeviceFragment,INameable
    {
        IRecordTemplate RecordTemplate { get; set; }
        IJournalLoadingSequence JournalLoadingSequence { get; set; }
        List<IJournalRecord> JournalRecords { get; set; }
    }
}