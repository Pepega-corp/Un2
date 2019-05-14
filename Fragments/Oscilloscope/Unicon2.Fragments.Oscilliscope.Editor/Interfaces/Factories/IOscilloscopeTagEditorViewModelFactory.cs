using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;

namespace Unicon2.Fragments.Oscilliscope.Editor.Interfaces.Factories
{
    public interface IOscilloscopeTagEditorViewModelFactory
    {
        IOscilloscopeTagEditorViewModel CreateOscilloscopeTagEditorViewModel(IOscilloscopeTag oscilloscopeTag);
        IOscilloscopeTagEditorViewModel CreateOscilloscopeTagEditorViewModel();

    }
}