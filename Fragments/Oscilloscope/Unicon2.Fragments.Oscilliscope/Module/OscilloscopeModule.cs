using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel;
using Unicon2.Fragments.Oscilliscope.Model;
using Unicon2.Fragments.Oscilliscope.ViewModel;
using Unicon2.Fragments.Oscilliscope.ViewModel.Journal;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Module
{
    public class OscilloscopeModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IDeviceFragment, OscilloscopeModel>(OscilloscopeKeys.OSCILLOSCOPE);
            container.Register<IFragmentViewModel, OscilloscopeViewModel>(OscilloscopeKeys.OSCILLOSCOPE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<IOscilloscopeLoadingSequenceInitializingParameters, OscilloscopeLoadingSequenceInitializingParameters>();
            container.Register<IOscilloscopeModel, OscilloscopeModel>();

            container.Register<IOscilloscopeJournalViewModel, OscilloscopeJournalViewModel>();
            container.Register<IOscilloscopeViewModel, OscilloscopeViewModel>();
            container.Register<IOscillogramLoadingParameters, OscillogramLoadingParameters>();
            container.Register<IOscilloscopeTag, OscilloscopeTag>();
            container.Register<ICountingTemplate, CountingTemplate>();
            container.Register<IJournalLoadingSequence, OscilloscopeJournalLoadingSequence>(OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE);
        
            /*ISerializerService serializerService = container.Resolve<ISerializerService>();
            serializerService.AddKnownTypeForSerialization(typeof(OscilloscopeModel));
            serializerService.AddNamespaceAttribute("oscilloscopeModel", "OscilloscopeModelNS");
            serializerService.AddNamespaceAttribute("oscillogram", "OscillogramNS");
            serializerService.AddKnownTypeForSerialization(typeof(OscilloscopeJournalLoadingSequence));
            serializerService.AddNamespaceAttribute("oscilloscopeJournalLoadingSequence", "OscilloscopeJournalLoadingSequenceNS");
            serializerService.AddKnownTypeForSerialization(typeof(OscillogramLoadingParameters));
            serializerService.AddNamespaceAttribute("oscillogramLoadingParameters", "OscillogramLoadingParametersNS");
            serializerService.AddKnownTypeForSerialization(typeof(OscilloscopeTag));
            serializerService.AddNamespaceAttribute("oscilloscopeTag", "OscilloscopeTagNS");
            serializerService.AddKnownTypeForSerialization(typeof(CountingTemplate));
            serializerService.AddNamespaceAttribute("countingTemplate", "CountingTemplateNS");*/

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/OscilloscopeDataTemplates.xaml",
                this.GetType().Assembly);
        }
    }
}
