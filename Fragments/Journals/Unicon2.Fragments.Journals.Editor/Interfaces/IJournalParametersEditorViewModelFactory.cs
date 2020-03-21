using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Interfaces
{
    public interface IJournalParametersEditorViewModelFactory
    {

        IJournalParameterEditorViewModel CreateJournalParameterEditorViewModel();
        IJournalParameterEditorViewModel CreateJournalParameterEditorViewModel(IJournalParameter journalParameter);


        IComplexJournalParameterEditorViewModel CreateComplexJournalParameterEditorViewModel();

        IComplexJournalParameterEditorViewModel CreateComplexJournalParameterEditorViewModel(
            IComplexJournalParameter complexJournalParameter);



        ISubJournalParameterEditorViewModel CreateJournalSubParameterEditorViewModel(
            ISubJournalParameter subJournalParameter, ObservableCollection<ISharedBitViewModel> sharedBitViewModels);

        ISubJournalParameterEditorViewModel CreateJournalSubParameterEditorViewModel(
            ObservableCollection<ISharedBitViewModel> sharedBitViewModels);



        IDependentJournalParameterEditorViewModel CreateDependentJournalParameterEditorViewModel(
            IDependentJournalParameter dependentJournalParameter, List<IJournalParameter> availableJournalParameters);

        IDependentJournalParameterEditorViewModel CreateDependentJournalParameterEditorViewModel(
            List<IJournalParameter> availableJournalParameters);

    }
}