using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Journals.Model.JournalLoadingSequence
{
    [DataContract(Namespace = "OffsetLoadingSequenceNS")]
    public class OffsetLoadingSequence : IOffsetLoadingSequence
    {
        private IDataProvider _dataProvider;
        private int _currentRecordIndex = 0;

        #region Implementation of IDataProviderContaining

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        #endregion

        #region Implementation of IJournalLoadingSequence

        public void Initialize(IJournalSequenceInitializingParameters journalSequenceInitializingParameters)
        {

        }

        public async Task<bool> GetIsNextRecordAvailable()
        {
            return this._currentRecordIndex < this.NumberOfRecords;

        }

        public async Task<ushort[]> GetNextRecordUshorts()
        {

            ushort readingAddress = (ushort)(this._currentRecordIndex * this.NumberOfPointsInRecord + this.JournalStartAddress);
            if (this.IsWordFormatNotForTheWholeRecord) readingAddress += this.WordFormatFrom;

            IQueryResult<ushort[]> queryResult = await this._dataProvider.ReadHoldingResgistersAsync(readingAddress,
                (ushort)(this.WordFormatTo + 1 - this.WordFormatFrom),
                JournalKeys.JOURNAL_RECORD_READING_QUERY);
            this._currentRecordIndex++;
            return queryResult.Result;
        }

        public void ResetSequence()
        {
            this._currentRecordIndex = 0;
        }

        #endregion

        #region Implementation of IOffsetLoadingSequence

        [DataMember]
        public int NumberOfRecords { get; set; }

        [DataMember]
        public ushort JournalStartAddress { get; set; }

        [DataMember]
        public ushort NumberOfPointsInRecord { get; set; }

        [DataMember]
        public ushort WordFormatFrom { get; set; }

        [DataMember]
        public ushort WordFormatTo { get; set; }

        [DataMember]
        public bool IsWordFormatNotForTheWholeRecord { get; set; }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => JournalKeys.OFFSET_LOADING_SEQUENCE;

        #endregion
    }
}
