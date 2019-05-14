using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Factories
{
    public class MatrixVariableOptionTemplateEditorViewModelFactory : IMatrixVariableOptionTemplateEditorViewModelFactory
    {
        private ITypesContainer _container;

        public MatrixVariableOptionTemplateEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public List<IMatrixVariableOptionTemplateEditorViewModel> CreateAvailableMatrixVariableOptionTemplateEditorViewModel()
        {
            List<IMatrixVariableOptionTemplateEditorViewModel> matrixVariableOptionTemplateEditorViewModels =
                new List<IMatrixVariableOptionTemplateEditorViewModel>();

            IEnumerable<IMatrixVariableOptionTemplate> matrixVariableOptionTemplates = this._container.ResolveAll<IMatrixVariableOptionTemplate>();

            matrixVariableOptionTemplates.ForEach(model =>
            {
                IMatrixVariableOptionTemplateEditorViewModel matrixVariableOptionTemplateEditorViewModel = this._container.Resolve<IViewModel>(
                        model.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IMatrixVariableOptionTemplateEditorViewModel;
                matrixVariableOptionTemplateEditorViewModel.Model = model;
                matrixVariableOptionTemplateEditorViewModels.Add(matrixVariableOptionTemplateEditorViewModel);
            });

            return matrixVariableOptionTemplateEditorViewModels;
        }
    }
}
