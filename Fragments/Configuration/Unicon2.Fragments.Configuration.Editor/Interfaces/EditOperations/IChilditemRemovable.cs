using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations
{
    public interface IChildItemRemovable
    {
        void RemoveChildItem(IEditorConfigurationItemViewModel configurationItemViewModelToRemove);
    }
}