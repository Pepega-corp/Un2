using System.Collections.Generic;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Factories
{
    public class JournalConditionEditorViewModelFactory : IJournalConditionEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public JournalConditionEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }


        public IJournalConditionEditorViewModel CreateJournalConditionEditorViewModel(List<IJournalParameter> availableJournalParameters)
        {
            IJournalConditionEditorViewModel journalConditionEditorViewModel =
                this._container.Resolve<IJournalConditionEditorViewModel>();
            journalConditionEditorViewModel.SetAvailablePatameters(availableJournalParameters);
            journalConditionEditorViewModel.ConditionsList = new List<string>() { ConditionsEnum.HaveFalseBitAt.ToString(), ConditionsEnum.HaveTrueBitAt.ToString(), ConditionsEnum.Equal.ToString() };
            return journalConditionEditorViewModel;
        }

        public IJournalConditionEditorViewModel CreateJournalConditionEditorViewModel(IJournalCondition journalCondition, List<IJournalParameter> availableJournalParameters)
        {
            IJournalConditionEditorViewModel journalConditionEditorViewModel =
                this._container.Resolve<IJournalConditionEditorViewModel>();
            journalConditionEditorViewModel.SetAvailablePatameters(availableJournalParameters);
            journalConditionEditorViewModel.ConditionsList = new List<string>() { ConditionsEnum.HaveFalseBitAt.ToString(), ConditionsEnum.HaveTrueBitAt.ToString(), ConditionsEnum.Equal.ToString() };
            journalConditionEditorViewModel.Model = journalCondition;
            return journalConditionEditorViewModel;
        }
    }
}
