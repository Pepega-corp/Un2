using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories
{
    public interface IVariableSignatureEditorViewModelFactory
    {
        IVariableSignatureEditorViewModel CreateVariableSignatureEditorViewModel(IVariableColumnSignature variableColumnSignature);
        IVariableSignatureEditorViewModel CreateVariableSignatureEditorViewModel();

    }
}