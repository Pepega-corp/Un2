using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Resources
{
    public interface IResourceViewModel<T> : INameable, IEditable, IStronglyNamed
    {
        string ResourceStrongName { get; set; }
        T RelatedEditorItemViewModel { get; set; }
    }
}