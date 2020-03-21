using Unicon2.Fragments.Journals.Infrastructure.Model;

namespace Unicon2.Fragments.Journals.Infrastructure.Factories
{
    public interface IJournalRecordFactory
    {
       IJournalRecord CreateJournalRecord(ushort[] values, IRecordTemplate recordTemplate);
    }
}