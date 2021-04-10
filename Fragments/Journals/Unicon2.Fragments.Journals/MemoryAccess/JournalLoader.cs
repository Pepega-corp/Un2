using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Factory;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public class JournalLoader : IJournalLoader
    {
        private readonly DeviceContext _deviceContext;
        private readonly IUniconJournal _uniconJournal;
        private readonly LoaderHooks _loaderHooks;
        private readonly IJournalRecordFactory _journalRecordFactory;
        private readonly IValueViewModelFactory _valueViewModelFactory;

        public JournalLoader(DeviceContext deviceContext, IUniconJournal uniconJournal, LoaderHooks loaderHooks)
        {
            _deviceContext = deviceContext;
            _uniconJournal = uniconJournal;
            _loaderHooks = loaderHooks;
            _journalRecordFactory = StaticContainer.Container.Resolve<IJournalRecordFactory>();
            _valueViewModelFactory = StaticContainer.Container.Resolve<IValueViewModelFactory>();
        }

        public void LoadFromReadyModelList(List<IJournalRecord> journalRecords)
        {
            _uniconJournal.JournalRecords.Clear();
            _loaderHooks.OnBeforeLoading();
            foreach (var journalRecord in journalRecords)
            {
                _uniconJournal.JournalRecords.Add(journalRecord);
                _loaderHooks.OnRecordValuesLoaded(journalRecord.FormattedValues);
            }
        }

        public async Task Load()
        {
            _uniconJournal.JournalRecords.Clear();
            var sequenceLoader =
                StaticContainer.Container.Resolve<ILoadingSequenceLoaderRegistry>().ResolveLoader(
                    _uniconJournal.JournalLoadingSequence,
                    _deviceContext);
            _loaderHooks.OnBeforeLoading();

            while (sequenceLoader.GetIsNextRecordAvailable())
            {
                var recordValues = await sequenceLoader.GetNextRecordUshorts();
                if (!recordValues.IsSuccess)
                {
                    break;
                }

                IJournalRecord newRec =
                    await _journalRecordFactory.CreateJournalRecord(recordValues.Item, _uniconJournal.RecordTemplate,
                        _deviceContext);
                if (newRec != null)
                {
                    _uniconJournal.JournalRecords.Add(newRec);
                    _loaderHooks.OnRecordValuesLoaded(newRec.FormattedValues);
                }
            }
        }


    }
}