using Unicon2.Fragments.Configuration.Editor.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations
{
    public interface IChildItemRemovable
    {
        void RemoveChildItem(IEditorConfigurationItemViewModel configurationItemViewModelToRemove);
    }
}