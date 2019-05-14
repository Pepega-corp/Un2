using System.Collections.Generic;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;

namespace Unicon2.Fragments.Journals.Editor.Interfaces
{
    public interface IJournalConditionEditorViewModelFactory
    {
        IJournalConditionEditorViewModel CreateJournalConditionEditorViewModel(List<IJournalParameter> availableJournalParameters);
        IJournalConditionEditorViewModel CreateJournalConditionEditorViewModel(IJournalCondition journalCondition, List<IJournalParameter> availableJournalParameters);

    }
}