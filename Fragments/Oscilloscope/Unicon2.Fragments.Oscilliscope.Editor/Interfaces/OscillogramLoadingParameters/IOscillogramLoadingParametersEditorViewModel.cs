using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters
{
    public interface IOscillogramLoadingParametersEditorViewModel:IViewModel
    {
        ObservableCollection<IOscilloscopeTagEditorViewModel> OscilloscopeTagEditorViewModels { get; }
        ushort AddressOfOscillogram { get; set; }
        ushort MaxSizeOfRewritableOscillogramInMs { get; set; }

        bool IsFullPageLoading { get; set; }
        ICommand AddTagCommand { get; }
        ICommand DeleteTagCommand { get; }
        void SetAvailableParameters(List<IJournalParameter> journalParameters);
    }
}