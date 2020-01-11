using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Factories
{
    public class MatrixMemoryVariableEditorViewModelFactory : IMatrixMemoryVariableEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public MatrixMemoryVariableEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }


        public IMatrixMemoryVariableEditorViewModel CreateMatrixMemoryVariableEditorViewModel(IMatrixMemoryVariable model)
        {
            IMatrixMemoryVariableEditorViewModel matrixMemoryVariableEditorViewModel =
                this._container.Resolve<IMatrixMemoryVariableEditorViewModel>();
            matrixMemoryVariableEditorViewModel.Model = model;
            return matrixMemoryVariableEditorViewModel;
        }

        public IMatrixMemoryVariableEditorViewModel CreateMatrixMemoryVariableEditorViewModel()
        {
            IMatrixMemoryVariableEditorViewModel matrixMemoryVariableEditorViewModel =
                this._container.Resolve<IMatrixMemoryVariableEditorViewModel>();
            matrixMemoryVariableEditorViewModel.Model = this._container.Resolve<IMatrixMemoryVariable>();
            return matrixMemoryVariableEditorViewModel;
        }
    }
}
