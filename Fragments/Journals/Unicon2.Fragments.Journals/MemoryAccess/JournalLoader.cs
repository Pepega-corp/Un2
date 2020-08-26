using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Factory;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class JournalLoader
    {
        private readonly IUniconJournalViewModel _uniconJournalViewModel;
        private readonly IDataProviderContainer _dataProviderContainer;
        private readonly IUniconJournal _uniconJournal;
        private readonly IJournalRecordFactory _journalRecordFactory;
        private readonly IValueViewModelFactory _valueViewModelFactory;


        public JournalLoader(IUniconJournalViewModel uniconJournalViewModel,
            IDataProviderContainer dataProviderContainer, IUniconJournal uniconJournal)
        {
            _uniconJournalViewModel = uniconJournalViewModel;
            _dataProviderContainer = dataProviderContainer;
            _uniconJournal = uniconJournal;
            _journalRecordFactory = StaticContainer.Container.Resolve<IJournalRecordFactory>();
            _valueViewModelFactory = StaticContainer.Container.Resolve<IValueViewModelFactory>();

        }

        public void LoadFromReadyModelList(List<IJournalRecord> journalRecords)
        {
	        _uniconJournal.JournalRecords.Clear();
	        _uniconJournalViewModel.Table.Values.Clear();
            foreach (var journalRecord in journalRecords)
	        {
		        _uniconJournal.JournalRecords.Add(journalRecord);
		        _uniconJournalViewModel.Table.AddFormattedValueViewModel(journalRecord.FormattedValues
			        .Select((formattedValue =>
				        _valueViewModelFactory.CreateFormattedValueViewModel(formattedValue))).ToList());
            }
        }

        public async Task Load()
        {
            _uniconJournal.JournalRecords.Clear();
            var sequenceLoader =
                new SequenceLoaderFactory().CreateSequenceLoader(_uniconJournal.JournalLoadingSequence,
                    _dataProviderContainer);
            while (sequenceLoader.GetIsNextRecordAvailable())
            {
                var recordValues = await sequenceLoader.GetNextRecordUshorts();

                IJournalRecord newRec =
                    _journalRecordFactory.CreateJournalRecord(recordValues, _uniconJournal.RecordTemplate);
                if (newRec != null)
                {
                    _uniconJournal.JournalRecords.Add(newRec);
                    _uniconJournalViewModel.Table.AddFormattedValueViewModel(newRec.FormattedValues
                        .Select((formattedValue =>
                            _valueViewModelFactory.CreateFormattedValueViewModel(formattedValue))).ToList());
                }
            }
            //TODO
            //List<ILoadable> loadables = new List<ILoadable>();
            //foreach (IJournalParameter parameter in _uniconJournal.RecordTemplate.JournalParameters)
            //{
            //    if (parameter.UshortsFormatter is ILoadable)
            //    {
            //        if (!loadables.Contains(parameter.UshortsFormatter as ILoadable))
            //            loadables.Add(parameter.UshortsFormatter as ILoadable);
            //    }
            //}

            //loadables.ForEach((async loadable => await loadable.Load()));
        }


    }
}