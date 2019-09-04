using Unicon2.Fragments.Journals.Editor.Factories;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Editor.ViewModel;
using Unicon2.Fragments.Journals.Editor.ViewModel.JournalParameters;
using Unicon2.Fragments.Journals.Editor.ViewModel.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Module
{
    public class JournaEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IFragmentEditorViewModel), typeof(UniconJournalEditorViewModel),
                JournalKeys.UNICON_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Register<IJournalParameterEditorViewModel, JournalParameterEditorViewModel>();
            container.Register<IJournalParametersEditorViewModelFactory, JournalParametersEditorViewModelFactory>();
            container.Register<IComplexJournalParameterEditorViewModel, ComplexJournalParameterEditorViewModel>();
            container.Register<IDependentJournalParameterEditorViewModel, DependentJournalParameterEditorViewModel>();
            container.Register<IJournalConditionEditorViewModel, JournalConditionEditorViewModel>();
            container.Register<ISubJournalParameterEditorViewModel, SubJournalParameterEditorViewModel>();
            container.Register<IJournalLoadingSequenceEditorViewModel, OffsetLoadingSequenceEditorViewModel>(JournalKeys.OFFSET_LOADING_SEQUENCE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<IJournalLoadingSequenceEditorViewModel, IndexLoadingSequenceEditorViewModel>(JournalKeys.INDEX_LOADING_SEQUENCE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<IUniconJournalEditorViewModel, UniconJournalEditorViewModel>();

            container.Register<IJournalConditionEditorViewModelFactory, JournalConditionEditorViewModelFactory>();

            container.Register<IJournalSequenceEditorViewModelFactory, JournalSequenceEditorViewModelFactory>();
            container.Register<IRecordTemplateEditorViewModel, RecordTemplateEditorViewModel>();

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/JournalDataTemplates.xaml", this.GetType().Assembly);
        }
    }
}
