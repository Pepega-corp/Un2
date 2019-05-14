using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.Factories;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Editor.Factories
{
    public class OscilloscopeTagEditorViewModelFactory : IOscilloscopeTagEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public OscilloscopeTagEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        #region Implementation of IOscilloscopeTagEditorViewModelFactory

        public IOscilloscopeTagEditorViewModel CreateOscilloscopeTagEditorViewModel(IOscilloscopeTag oscilloscopeTag)
        {
            IOscilloscopeTagEditorViewModel oscilloscopeTagEditorViewModel = this._container.Resolve<IOscilloscopeTagEditorViewModel>();
            oscilloscopeTagEditorViewModel.Model = oscilloscopeTag;
            return oscilloscopeTagEditorViewModel;
        }

        public IOscilloscopeTagEditorViewModel CreateOscilloscopeTagEditorViewModel()
        {
            IOscilloscopeTagEditorViewModel oscilloscopeTagEditorViewModel = this._container.Resolve<IOscilloscopeTagEditorViewModel>();
            IOscilloscopeTag oscilloscopeTag = this._container.Resolve<IOscilloscopeTag>();
            oscilloscopeTagEditorViewModel.Model = oscilloscopeTag;
            return oscilloscopeTagEditorViewModel;
        }

        #endregion
    }
}
