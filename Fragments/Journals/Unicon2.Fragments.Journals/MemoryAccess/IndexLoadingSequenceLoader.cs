using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Model.JournalLoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class IndexLoadingSequenceLoader : ISequenceLoader
    {
        private readonly IndexLoadingSequence _indexLoadingSequence;
        private readonly IDataProviderContaining _dataProviderContaining;
        private ushort _currentRecordIndex = 0;
        private int _lastQueryErrorCode = 1; // code "0" means last record

        public IndexLoadingSequenceLoader(IndexLoadingSequence indexLoadingSequence,
            IDataProviderContaining dataProviderContaining)
        {
            _indexLoadingSequence = indexLoadingSequence;
            _dataProviderContaining = dataProviderContaining;

        }

        public bool GetIsNextRecordAvailable()
        {
            return _lastQueryErrorCode != 0;
        }

        public async Task<ushort[]> GetNextRecordUshorts()
        {
            await _dataProviderContaining.DataProvider.WriteSingleRegisterAsync(
                _indexLoadingSequence.JournalStartAddress, _currentRecordIndex, "Write");
            _currentRecordIndex++;

            IQueryResult<ushort[]> queryResult = await _dataProviderContaining.DataProvider.ReadHoldingResgistersAsync(
                _indexLoadingSequence.JournalStartAddress,
                _indexLoadingSequence.NumberOfPointsInRecord,
                JournalKeys.JOURNAL_RECORD_READING_QUERY);
            _lastQueryErrorCode = queryResult.Result[queryResult.Result.Length - 1];
            return queryResult.Result;
        }
    }
}