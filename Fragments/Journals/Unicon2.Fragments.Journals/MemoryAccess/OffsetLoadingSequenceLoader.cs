using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Model.JournalLoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class OffsetLoadingSequenceLoader : ISequenceLoader
    {
        private readonly OffsetLoadingSequence _offsetLoadingSequence;
        private readonly IDataProviderContainer _dataProviderContainer;

        public OffsetLoadingSequenceLoader(OffsetLoadingSequence offsetLoadingSequence,
            IDataProviderContainer dataProviderContainer)
        {
            _offsetLoadingSequence = offsetLoadingSequence;
            _dataProviderContainer = dataProviderContainer;
        }

        private int _currentRecordIndex = 0;

        public bool GetIsNextRecordAvailable()
        {
            return _currentRecordIndex < _offsetLoadingSequence.NumberOfRecords;

        }

        public async Task<Result<ushort[]>> GetNextRecordUshorts()
        {
            if (!_dataProviderContainer.DataProvider.IsSuccess)
            {
                return Result<ushort[]>.Create(false);
            }
            ushort readingAddress = (ushort) (_currentRecordIndex * _offsetLoadingSequence.NumberOfPointsInRecord +
                                              _offsetLoadingSequence.JournalStartAddress);
            if (_offsetLoadingSequence.IsWordFormatNotForTheWholeRecord)
                readingAddress += _offsetLoadingSequence.WordFormatFrom;

            IQueryResult<ushort[]> queryResult = await _dataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(
                readingAddress,
                (ushort) (_offsetLoadingSequence.WordFormatTo - _offsetLoadingSequence.WordFormatFrom),
                JournalKeys.JOURNAL_RECORD_READING_QUERY);
            if (!queryResult.IsSuccessful)
            {
                return Result<ushort[]>.Create(false);
            }
            _currentRecordIndex++;
            return Result<ushort[]>.Create(queryResult.Result,true);
        }
    }
}
