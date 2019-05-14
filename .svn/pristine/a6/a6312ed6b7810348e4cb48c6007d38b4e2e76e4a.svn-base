using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Oscilliscope.Editor.Interfaces
{
    public interface IOscilloscopeEditorViewModel:IFragmentEditorViewModel
    {
        IUniconJournalEditorViewModel OscilloscopeJournalEditorViewModel { get; set; }
        IOscillogramLoadingParametersEditorViewModel OscillogramLoadingParametersEditorViewModel { get; set; }
        IRecordTemplateEditorViewModel CountingTemplateEditorViewModel { get; set; }
        bool IsOscilloscopeJournalTabSelected { get; set; }
    }
}