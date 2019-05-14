using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel
{
    public interface IAssignedBitEditorViewModel : IViewModel
    {
        int NumberOfBit { get; set; }
        IBitOptionEditorViewModel SelectedBitOptionEditorViewModel { get; set; }
        ObservableCollection<IBitOptionEditorViewModel> BitOptionEditorViewModels { get; set; }
        bool IsBitAssigned { get; }
        ICommand ResetCommand { get; }
    }
}