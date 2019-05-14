using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters
{
    public interface IOscilloscopeTagEditorViewModel : IViewModel
    {
        void SetAvailableOptions(List<IJournalParameter> journalParameters,List<string> tags);
        ObservableCollection<string> AvailableJournalParameters { get; }
        ObservableCollection<string> AvailableTags { get; }
        string SelectedTag { get; set; }
        string SelectedJournalParameter { get; set; }
    }
}
