using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations
{
    public interface IChildItemRemovable
    {
        void RemoveChildItem(IConfigurationItem configurationItemToRemove);
    }
}