using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Factories
{
    public class JournalParametersEditorViewModelFactory : IJournalParametersEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public JournalParametersEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IJournalParameterEditorViewModel CreateJournalParameterEditorViewModel(IJournalParameter journalParameter)
        {
            IJournalParameterEditorViewModel journalParameterEditorViewModel = this._container.Resolve<IJournalParameterEditorViewModel>();
            journalParameterEditorViewModel.Model = journalParameter;
            return journalParameterEditorViewModel;
        }

        public IJournalParameterEditorViewModel CreateJournalParameterEditorViewModel()
        {
            IJournalParameterEditorViewModel journalParameterEditorViewModel = this._container.Resolve<IJournalParameterEditorViewModel>();
            journalParameterEditorViewModel.Address = "0";
            journalParameterEditorViewModel.NumberOfPoints = "1";

            return journalParameterEditorViewModel;
        }

        public IComplexJournalParameterEditorViewModel CreateComplexJournalParameterEditorViewModel(
            IComplexJournalParameter complexJournalParameter)
        {
            IComplexJournalParameterEditorViewModel journalParameterEditorViewModel = this._container.Resolve<IComplexJournalParameterEditorViewModel>();
            journalParameterEditorViewModel.Model = complexJournalParameter;
            return journalParameterEditorViewModel;
        }

        public IComplexJournalParameterEditorViewModel CreateComplexJournalParameterEditorViewModel()
        {
            IComplexJournalParameterEditorViewModel journalParameterEditorViewModel = this._container.Resolve<IComplexJournalParameterEditorViewModel>();
            journalParameterEditorViewModel.Address = "0";
            journalParameterEditorViewModel.NumberOfPoints = "1";
            return journalParameterEditorViewModel;
        }

        public ISubJournalParameterEditorViewModel CreateJournalSubParameterEditorViewModel(
            ISubJournalParameter subJournalParameter, ObservableCollection<ISharedBitViewModel> sharedBitViewModels)
        {
            ISubJournalParameterEditorViewModel journalParameterEditorViewModel = this._container.Resolve<ISubJournalParameterEditorViewModel>();
            journalParameterEditorViewModel.BitNumbersInWord = sharedBitViewModels;
            journalParameterEditorViewModel.Model = subJournalParameter;
            return journalParameterEditorViewModel;
        }

        public ISubJournalParameterEditorViewModel CreateJournalSubParameterEditorViewModel(ObservableCollection<ISharedBitViewModel> sharedBitViewModels)
        {
            ISubJournalParameterEditorViewModel journalParameterEditorViewModel = this._container.Resolve<ISubJournalParameterEditorViewModel>();
            journalParameterEditorViewModel.BitNumbersInWord = sharedBitViewModels;
            journalParameterEditorViewModel.Address = "0";
            journalParameterEditorViewModel.NumberOfPoints = "1";
            return journalParameterEditorViewModel;
        }

        public IDependentJournalParameterEditorViewModel CreateDependentJournalParameterEditorViewModel(
            IDependentJournalParameter dependentJournalParameter, List<IJournalParameter> availableJournalParameters)
        {
            IDependentJournalParameterEditorViewModel dependentJournalParameterEditorViewModel = this._container.Resolve<IDependentJournalParameterEditorViewModel>();
            dependentJournalParameterEditorViewModel.Address = "0";
            dependentJournalParameterEditorViewModel.NumberOfPoints = "1";
            dependentJournalParameterEditorViewModel.SetAvaliableJournalParameters(availableJournalParameters);

            dependentJournalParameterEditorViewModel.Model = dependentJournalParameter;
            return dependentJournalParameterEditorViewModel;
        }

        public IDependentJournalParameterEditorViewModel CreateDependentJournalParameterEditorViewModel(List<IJournalParameter> availableJournalParameters)
        {
            IDependentJournalParameterEditorViewModel dependentJournalParameterEditorViewModel = this._container.Resolve<IDependentJournalParameterEditorViewModel>();
            dependentJournalParameterEditorViewModel.Address = "0";
            dependentJournalParameterEditorViewModel.NumberOfPoints = "1";
            dependentJournalParameterEditorViewModel.SetAvaliableJournalParameters(availableJournalParameters);

            return dependentJournalParameterEditorViewModel;
        }
    }
}
