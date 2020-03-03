using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces.EditOperations;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IConfigurationGroupEditorViewModel : IEditorConfigurationItemViewModel, IItemGroupViewModel,
        IAddressIncreaseableDecreaseable, ICompositeEditOperations, IChildPositionChangeable, IChildItemRemovable,
        IAsChildPasteable
    {
        bool IsMain { get; set; }
        bool IsGroupWithReiteration { get; set; }

    }
}