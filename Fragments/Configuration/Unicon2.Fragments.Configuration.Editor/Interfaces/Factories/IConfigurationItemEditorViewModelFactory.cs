using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Factories
{
    public interface
        IConfigurationItemEditorViewModelFactory : IConfigurationItemVisitor<IEditorConfigurationItemViewModel>
    {

    }
}