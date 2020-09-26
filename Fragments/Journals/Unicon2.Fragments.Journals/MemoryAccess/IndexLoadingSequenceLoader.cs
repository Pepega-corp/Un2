using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Model.JournalLoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class IndexLoadingSequenceLoader : ISequenceLoader
    {
        private readonly IndexLoadingSequence _indexLoadingSequence;
        private readonly IDataProviderContainer _dataProviderContainer;
        private ushort _currentRecordIndex = 0;
        private int _lastQueryErrorCode = 1; // code "0" means last record

        public IndexLoadingSequenceLoader(IndexLoadingSequence indexLoadingSequence,
            IDataProviderContainer dataProviderContainer)
        {
            _indexLoadingSequence = indexLoadingSequence;
            _dataProviderContainer = dataProviderContainer;

        }

        public bool GetIsNextRecordAvailable()
        {
            return _lastQueryErrorCode != 0;
        }

        public async Task<Result<ushort[]>> GetNextRecordUshorts()
        {
           var res= await _dataProviderContainer.DataProvider.WriteSingleRegisterAsync(
                _indexLoadingSequence.IndexWritingAddress, _currentRecordIndex, "Write");
            _currentRecordIndex++;

            
            IQueryResult<ushort[]> queryResult = await _dataProviderContainer.DataProvider.ReadHoldingResgistersAsync(
                _indexLoadingSequence.JournalStartAddress,
                _indexLoadingSequence.NumberOfPointsInRecord,
                JournalKeys.JOURNAL_RECORD_READING_QUERY);
            if (!queryResult.IsSuccessful)
            {
                return Result<ushort[]>.Create(false);
            }
            _lastQueryErrorCode = queryResult.Result[0];
            return Result<ushort[]>.Create(queryResult.Result, true);
        }
    }
}