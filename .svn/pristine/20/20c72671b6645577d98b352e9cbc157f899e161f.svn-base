using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel
{
    public interface IMatrixTemplateEditorViewModel : IViewModel
    {
        int NumberOfBitsOnEachVariable { get; set; }
        ObservableCollection<IMatrixMemoryVariableEditorViewModel> MatrixMemoryVariableEditorViewModels { get; }
        ObservableCollection<IVariableSignatureEditorViewModel> VariableSignatureEditorViewModels { get; }

        List<IMatrixVariableOptionTemplateEditorViewModel> AvailableMatrixVariableOptionTemplateEditorViewModels
        {
            get;
            set;
        }

        ObservableCollection<IBitOptionEditorViewModel> BitOptionEditorViewModels { get; }

        IMatrixVariableOptionTemplateEditorViewModel SelectedMatrixVariableOptionTemplateEditorViewModel { get; set; }
        ObservableCollection<IAssignedBitEditorViewModel> AssignedBitEditorViewModels { get; }
        ICommand OnSelectionChangedCommand { get; }

        ICommand AddMatrixVariableCommand { get; }
        ICommand AddSignatureCommand { get; }

        ICommand DeleteMatrixVariableCommand { get; }
        ICommand DeleteSignatureCommand { get; }

        ICommand SubmitCommand { get; }
        ICommand CancelCommand { get; }

        string MatrixName { get; set; }
    }
}