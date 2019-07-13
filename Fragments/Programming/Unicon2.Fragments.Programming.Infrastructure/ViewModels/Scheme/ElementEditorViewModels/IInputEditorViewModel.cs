using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels
{
    public interface IInputEditorViewModel : ILogicElementEditorViewModel
    {
        ObservableCollection<string> Bases { get; }
        bool NeedBases { get; set; }

        Dictionary<int, string> InputSignals { get; }
    }
}