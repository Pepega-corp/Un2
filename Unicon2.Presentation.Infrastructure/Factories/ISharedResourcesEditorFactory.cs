using Unicon2.Presentation.Infrastructure.ViewModels.Resources;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface ISharedResourcesEditorFactory
    {
        void OpenResourceForEdit(IResourceViewModel resource, object _owner);
        
    }
}