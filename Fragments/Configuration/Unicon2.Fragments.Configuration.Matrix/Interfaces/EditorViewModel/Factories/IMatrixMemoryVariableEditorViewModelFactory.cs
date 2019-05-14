using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories
{
    public interface IMatrixMemoryVariableEditorViewModelFactory
    {
        IMatrixMemoryVariableEditorViewModel CreateMatrixMemoryVariableEditorViewModel(IMatrixMemoryVariable model);
        IMatrixMemoryVariableEditorViewModel CreateMatrixMemoryVariableEditorViewModel();

    }
}