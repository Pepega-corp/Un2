using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Editor.Factories;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.Factories;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Editor.ViewModel;
using Unicon2.Fragments.Oscilliscope.Editor.ViewModel.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Editor.ViewModel.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Editor.Module
{
    public class OscilloscopeEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IFragmentEditorViewModel), typeof(OscilloscopeEditorViewModel),
                OscilloscopeKeys.OSCILLOSCOPE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Register(typeof(IJournalLoadingSequenceEditorViewModel), typeof(OscilloscopeJournalLoadingSequenceEditorViewModel),
                OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Register(typeof(IOscillogramLoadingParametersEditorViewModel), typeof(OscillogramLoadingParametersEditorViewModel));
            container.Register(typeof(IOscilloscopeTagEditorViewModel), typeof(OscilloscopeTagEditorViewModel));
            container.Register(typeof(IOscilloscopeTagEditorViewModelFactory), typeof(OscilloscopeTagEditorViewModelFactory));

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/OscilloscopeDataTemplates.xaml", GetType().Assembly);
        }
    }
}
