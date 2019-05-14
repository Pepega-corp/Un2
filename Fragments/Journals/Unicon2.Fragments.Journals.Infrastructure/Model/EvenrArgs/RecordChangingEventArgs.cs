namespace Unicon2.Fragments.Journals.Infrastructure.Model.EvenrArgs
{
   public class RecordChangingEventArgs
    {
        public IJournalRecord JournalRecord { get; set; }
        public RecordChangingEnum RecordChangingEnum { get; set; }
    }

}
