using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Model.JournalLoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class OffsetLoadingSequenceLoader : ISequenceLoader
    {
        private readonly OffsetLoadingSequence _offsetLoadingSequence;
        private readonly IDataProviderContaining _dataProviderContaining;

        public OffsetLoadingSequenceLoader(OffsetLoadingSequence offsetLoadingSequence,
            IDataProviderContaining dataProviderContaining)
        {
            _offsetLoadingSequence = offsetLoadingSequence;
            _dataProviderContaining = dataProviderContaining;
        }

        private int _currentRecordIndex = 0;

        public bool GetIsNextRecordAvailable()
        {
            return _currentRecordIndex < _offsetLoadingSequence.NumberOfRecords;

        }

        public async Task<ushort[]> GetNextRecordUshorts()
        {

            ushort readingAddress = (ushort) (_currentRecordIndex * _offsetLoadingSequence.NumberOfPointsInRecord +
                                              _offsetLoadingSequence.JournalStartAddress);
            if (_offsetLoadingSequence.IsWordFormatNotForTheWholeRecord)
                readingAddress += _offsetLoadingSequence.WordFormatFrom;

            IQueryResult<ushort[]> queryResult = await _dataProviderContaining.DataProvider.ReadHoldingResgistersAsync(
                readingAddress,
                (ushort) (_offsetLoadingSequence.WordFormatTo + 1 - _offsetLoadingSequence.WordFormatFrom),
                JournalKeys.JOURNAL_RECORD_READING_QUERY);
            _currentRecordIndex++;
            return queryResult.Result;
        }
    }
}
