using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public interface IConfigurationItemViewModelVisitor<TOutPut>
    {
        TOutPut VisitItemsGroup(IConfigurationGroupEditorViewModel itemsGroup);
        TOutPut VisitProperty(IPropertyEditorViewModel property);
        TOutPut VisitComplexProperty(IComplexPropertyEditorViewModel property);
        TOutPut VisitMatrix(IEditorConfigurationItemViewModel appointableMatrixViewModel);
        TOutPut VisitSubProperty(ISubPropertyEditorViewModel dependentPropertyViewModel);
    }
}