using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.EvenrArgs;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Model
{
    [DataContract(Namespace = "UniconJournalNS")]
    public class UniconJournal : IUniconJournal
    {
        private IJournalRecordFactory _journalRecordFactory;
        private IDataProvider _dataProvider;
        private IDeviceSharedResources _deviceSharedResources;

        public UniconJournal(IRecordTemplate recordTemplate, IJournalRecordFactory journalRecordFactory)
        {
            this._journalRecordFactory = journalRecordFactory;
            this.RecordTemplate = recordTemplate;
            this.JournalRecords = new List<IJournalRecord>();
        }

        [DataMember]
        public IRecordTemplate RecordTemplate { get; set; }

        [DataMember]
        public IJournalLoadingSequence JournalLoadingSequence { get; set; }

        [DataMember(Name = nameof(JournalRecords))]
        public List<IJournalRecord> JournalRecords { get; set; }

        public Action<RecordChangingEventArgs> JournalRecordsChanged { get; set; }

        public void SerializeInFile(string elementName, bool isDefaultSaving)
        {
            throw new NotImplementedException();
        }

        public void DeserializeFromFile(string path)
        {
            throw new NotImplementedException();
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
            this.JournalLoadingSequence.SetDataProvider(dataProvider);
        }

        public async Task Load()
        {
            if (this._dataProvider == null) return;
            this.JournalRecords = new List<IJournalRecord>();
            this.JournalLoadingSequence.ResetSequence();
            this.JournalRecordsChanged?.Invoke(
                new RecordChangingEventArgs() { RecordChangingEnum = RecordChangingEnum.RecordsRefreshed });
            this.JournalRecordsChanged?.Invoke(
                new RecordChangingEventArgs() { RecordChangingEnum = RecordChangingEnum.RecordsReadingStarted });
            List<ILoadable> loadables = new List<ILoadable>();
            foreach (IJournalParameter parameter in this.RecordTemplate.JournalParameters)
            {
                if (parameter.UshortsFormatter is ILoadable)
                {
                    if (!loadables.Contains(parameter.UshortsFormatter as ILoadable))
                        loadables.Add(parameter.UshortsFormatter as ILoadable);
                }
            }
            loadables.ForEach((async loadable => await loadable.Load()));
            await this.LoadJournalValues();
        }


        private async Task LoadJournalValues()
        {
            while (await this.JournalLoadingSequence.GetIsNextRecordAvailable())
            {
                ushort[] recordUshorts = await this.JournalLoadingSequence.GetNextRecordUshorts();
                IJournalRecord newRec =
                   await this._journalRecordFactory.CreateJournalRecord(recordUshorts, this.RecordTemplate);
                if (newRec != null)
                {
                    this.JournalRecords.Add(newRec);
                    this.JournalRecordsChanged?.Invoke(new RecordChangingEventArgs()
                    {
                        JournalRecord = newRec,
                        RecordChangingEnum = RecordChangingEnum.RecordAdded
                    });
                }
            }
            this.JournalRecordsChanged?.Invoke(new RecordChangingEventArgs()
            {
                RecordChangingEnum = RecordChangingEnum.RecordsReadingFinished
            });
        }

        public string StrongName => JournalKeys.UNICON_JOURNAL;

        public IFragmentSettings FragmentSettings { get; set; }

        [DataMember]
        public string Name { get; set; }

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            this.IsInitialized = true;
            this._journalRecordFactory = container.Resolve<IJournalRecordFactory>();
            this.RecordTemplate?.JournalParameters?.ForEach(
                (parameter => parameter.InitializeFromContainer(container)));
            (this.FragmentSettings as IInitializableFromContainer)?.InitializeFromContainer(container);

        }
    }
}