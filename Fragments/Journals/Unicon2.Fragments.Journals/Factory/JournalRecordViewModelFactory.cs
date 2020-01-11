using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Factory
{
    public class JournalRecordViewModelFactory : IJournalRecordViewModelFactory
    {
        private readonly ITypesContainer _container;

        public JournalRecordViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IJournalRecordViewModel CreateJournalRecordViewModel(IJournalRecord journalRecord)
        {
            IJournalRecordViewModel journalRecordViewModel = this._container.Resolve<IJournalRecordViewModel>();
            journalRecordViewModel.Model = journalRecord;
            return journalRecordViewModel;
        }
    }
}