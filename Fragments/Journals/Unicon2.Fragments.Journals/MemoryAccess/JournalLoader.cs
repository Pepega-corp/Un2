using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Factory;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class JournalLoader
    {
        private readonly IUniconJournalViewModel _uniconJournalViewModel;
        private readonly IDataProviderContaining _dataProviderContaining;
        private readonly IUniconJournal _uniconJournal;
        private readonly IJournalRecordFactory _journalRecordFactory;
        private readonly IValueViewModelFactory _valueViewModelFactory;


        public JournalLoader(IUniconJournalViewModel uniconJournalViewModel,
            IDataProviderContaining dataProviderContaining, IUniconJournal uniconJournal)
        {
            _uniconJournalViewModel = uniconJournalViewModel;
            _dataProviderContaining = dataProviderContaining;
            _uniconJournal = uniconJournal;
            _journalRecordFactory = StaticContainer.Container.Resolve<IJournalRecordFactory>();
            _valueViewModelFactory = StaticContainer.Container.Resolve<IValueViewModelFactory>();

        }


        public async Task Load()
        {
            _uniconJournal.JournalRecords.Clear();
            var sequenceLoader =
                new SequenceLoaderFactory().CreateSequenceLoader(_uniconJournal.JournalLoadingSequence,
                    _dataProviderContaining);
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