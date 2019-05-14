using System;
using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Model.EvenrArgs;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Journals.Infrastructure.Model
{
    public interface IUniconJournal:ISerializableInFile,ILoadable,IDeviceFragment,INameable,IInitializableFromContainer
    {
        IRecordTemplate RecordTemplate { get; set; }
        IJournalLoadingSequence JournalLoadingSequence { get; set; }
        List<IJournalRecord> JournalRecords { get; set; }
        Action<RecordChangingEventArgs> JournalRecordsChanged { get; set; }
    }
}