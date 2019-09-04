using Unicon2.Fragments.Journals.Factory;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Journals.Model;
using Unicon2.Fragments.Journals.Model.JournalLoadingSequence;
using Unicon2.Fragments.Journals.Model.JournalParameters;
using Unicon2.Fragments.Journals.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Module
{
    public class UniconJournalModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IUniconJournal, UniconJournal>();
            container.Register<IJournalRecord, JournalRecord>();
            container.Register<IJournalParameter, JournalParameter>();
            container.Register<IComplexJournalParameter, ComplexJournalParameter>();
            container.Register<ISubJournalParameter, SubJournalParameter>();
            container.Register<IDependentJournalParameter, DependentJournalParameter>();
            container.Register<IJournalCondition, JournalParameterDependancyCondition>();

            container.Register<IRecordTemplate, RecordTemplate>();

            container.Register<IFragmentViewModel, UniconJournalViewModel>(
                JournalKeys.UNICON_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IJournalRecordViewModel, JournalRecordViewModel>();
            container.Register<IJournalRecordFactory, JournalRecordFactory>();
            container.Register<IJournalRecordViewModelFactory, JournalRecordViewModelFactory>();

            container.Register<IJournalLoadingSequence, OffsetLoadingSequence>(JournalKeys
                .OFFSET_LOADING_SEQUENCE);
            container.Register<IJournalLoadingSequence, IndexLoadingSequence>(JournalKeys
                .INDEX_LOADING_SEQUENCE);

            ISerializerService serializerService = container.Resolve<ISerializerService>();
            serializerService.AddKnownTypeForSerializationRange(new[]
            {
                typeof(DependentJournalParameter), typeof(JournalParameterDependancyCondition),
                typeof(UniconJournal), typeof(JournalRecord), typeof(JournalParameter), typeof(RecordTemplate),
                typeof(SubJournalParameter), typeof(ComplexJournalParameter), typeof(OffsetLoadingSequence), typeof(IndexLoadingSequence)
            });
            serializerService.AddNamespaceAttribute("journal", "UniconJournalNS");
            serializerService.AddNamespaceAttribute("journalRecord", "JournalRecordNS");
            serializerService.AddNamespaceAttribute("journalParameter", "JournalParameterNS");
            serializerService.AddNamespaceAttribute("subJournalParameter", "SubJournalParameterNS");
            serializerService.AddNamespaceAttribute("complexJournalParameter", "ComplexJournalParameterNS");
            serializerService.AddNamespaceAttribute("offsetLoadingSequence", "OffsetLoadingSequenceNS");
            serializerService.AddNamespaceAttribute("indexLoadingSequence", "IndexLoadingSequenceNS");
            serializerService.AddNamespaceAttribute("recordTemplate", "RecordTemplateNS");
            serializerService.AddNamespaceAttribute("journalParameterDependancyCondition",
                "JournalParameterDependancyConditionNS");
            serializerService.AddNamespaceAttribute("dependentJournalParameter", "DependentJournalParameterNS");


            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/JournalDataTemplates.xaml",
                this.GetType().Assembly);
        }
    }
}
