namespace Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence
{
    public interface IOffsetLoadingSequence : IJournalLoadingSequence
    {
        int NumberOfRecords { get; set; }
        ushort JournalStartAddress { get; set; }
        ushort NumberOfPointsInRecord { get; set; }
        ushort WordFormatFrom { get; set; }
        ushort WordFormatTo { get; set; }
        bool IsWordFormatNotForTheWholeRecord { get; set; }
    }
}