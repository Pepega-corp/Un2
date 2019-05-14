using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;
using Unicon2.Infrastructure;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Factories
{
    public class JournalSequenceEditorViewModelFactory : IJournalSequenceEditorViewModelFactory
    {
        private ITypesContainer _container;

        public JournalSequenceEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IJournalLoadingSequenceEditorViewModel CreateJournalLoadingSequenceEditorViewModel(IJournalLoadingSequence journalLoadingSequence)
        {
            IJournalLoadingSequenceEditorViewModel journalLoadingSequenceEditorViewModel =
                this._container.Resolve<IJournalLoadingSequenceEditorViewModel>(journalLoadingSequence.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            journalLoadingSequenceEditorViewModel.Model = journalLoadingSequence;
            return journalLoadingSequenceEditorViewModel;
        }

        public List<IJournalLoadingSequenceEditorViewModel> GetAvailableLoadingSequenceEditorViewModels()
        {
            List<IJournalLoadingSequence> journalLoadingSequences = this._container.ResolveAll<IJournalLoadingSequence>().ToList();

            List<IJournalLoadingSequenceEditorViewModel> journalLoadingSequenceEditorViewModels = new List<IJournalLoadingSequenceEditorViewModel>();

            foreach (IJournalLoadingSequence journalLoadingSequence in journalLoadingSequences)
            {
                IJournalLoadingSequenceEditorViewModel viewmodel = this._container.Resolve<IJournalLoadingSequenceEditorViewModel>(
                    journalLoadingSequence.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
                viewmodel.Model = journalLoadingSequence;
                journalLoadingSequenceEditorViewModels.Add(viewmodel);
            }
            return journalLoadingSequenceEditorViewModels;
        }
    }
}
