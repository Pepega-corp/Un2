using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates
{
    public class BoolMatrixVariableOptionTemplateEditorViewModel : ViewModelBase, IMatrixVariableOptionTemplateEditorViewModel
    {
        public string StrongName => MatrixKeys.BOOL_MATRIX_TEMPLATE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model { get; set; }
    }
}
