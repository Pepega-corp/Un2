using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Factories
{
    public class VariableSignatureEditorViewModelFactory : IVariableSignatureEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public VariableSignatureEditorViewModelFactory(ITypesContainer container)
        {
            _container = container;
        }

        public IVariableSignatureEditorViewModel CreateVariableSignatureEditorViewModel(IVariableColumnSignature variableColumnSignature)
        {
            IVariableSignatureEditorViewModel variableSignatureEditorViewModel =
                _container.Resolve<IVariableSignatureEditorViewModel>();
            variableSignatureEditorViewModel.Model = variableColumnSignature;
            return variableSignatureEditorViewModel;
        }

        public IVariableSignatureEditorViewModel CreateVariableSignatureEditorViewModel()
        {
            IVariableSignatureEditorViewModel variableSignatureEditorViewModel =
                _container.Resolve<IVariableSignatureEditorViewModel>();
            variableSignatureEditorViewModel.Model = _container.Resolve<IVariableColumnSignature>();
            return variableSignatureEditorViewModel;
        }
    }
}
