using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Factory
{
    public class JournalRecordViewModelFactory : IJournalRecordViewModelFactory
    {
        private readonly ITypesContainer _container;
        private readonly IValueViewModelFactory _valueViewModelFactory;

        public JournalRecordViewModelFactory(ITypesContainer container, IValueViewModelFactory valueViewModelFactory)
        {
            _container = container;
            _valueViewModelFactory = valueViewModelFactory;
        }

        public IJournalRecordViewModel CreateJournalRecordViewModel(IJournalRecord journalRecord)
        {
            IJournalRecordViewModel journalRecordViewModel = _container.Resolve<IJournalRecordViewModel>();
            foreach (Unicon2.Infrastructure.Values.IFormattedValue formattedValue in journalRecord
                .FormattedValues)
            {
                journalRecordViewModel.FormattedValueViewModels.Add(
                    _valueViewModelFactory.CreateFormattedValueViewModel(formattedValue));
            }
            return journalRecordViewModel;
        }
    }
}