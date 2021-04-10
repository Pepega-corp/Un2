using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Journals.Infrastructure.Model.Loader
{
    public interface IJournalLoader
    {
        Task Load();
        void LoadFromReadyModelList(List<IJournalRecord> journalRecords);
    }

    public class LoaderHooks
    {
        public LoaderHooks(Action onBeforeLoading, Action<List<IFormattedValue>> onRecordValuesLoaded)
        {
            OnBeforeLoading = onBeforeLoading;
            OnRecordValuesLoaded = onRecordValuesLoaded;
        }

        public Action OnBeforeLoading { get; }
        public Action<List<IFormattedValue>> OnRecordValuesLoaded { get; }
    }
}