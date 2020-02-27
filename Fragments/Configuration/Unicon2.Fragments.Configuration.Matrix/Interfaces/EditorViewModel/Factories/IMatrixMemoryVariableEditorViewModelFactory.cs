using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories
{
    public interface IMatrixMemoryVariableEditorViewModelFactory
    {
        IMatrixMemoryVariableEditorViewModel CreateMatrixMemoryVariableEditorViewModel(IMatrixMemoryVariable model);
        IMatrixMemoryVariableEditorViewModel CreateMatrixMemoryVariableEditorViewModel();

    }
}