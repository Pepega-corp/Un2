namespace Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence
{
    public interface IIndexLoadingSequence : IJournalLoadingSequence
    {
        ushort JournalStartAddress { get; set; }
        ushort NumberOfPointsInRecord { get; set; }
        ushort WordFormatFrom { get; set; }
        ushort WordFormatTo { get; set; }
        bool IsWordFormatNotForTheWholeRecord { get; set; }
        ushort IndexWritingAddress { get; set; }
        bool WriteIndexOnlyFirstTime { get; set; }

    }
}