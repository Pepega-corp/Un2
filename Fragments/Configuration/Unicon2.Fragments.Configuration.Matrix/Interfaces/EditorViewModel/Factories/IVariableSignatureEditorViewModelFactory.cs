using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories
{
    public interface IVariableSignatureEditorViewModelFactory
    {
        IVariableSignatureEditorViewModel CreateVariableSignatureEditorViewModel(IVariableColumnSignature variableColumnSignature);
        IVariableSignatureEditorViewModel CreateVariableSignatureEditorViewModel();

    }
}