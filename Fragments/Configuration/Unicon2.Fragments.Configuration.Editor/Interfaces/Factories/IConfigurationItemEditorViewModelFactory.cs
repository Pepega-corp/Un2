using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Factories
{
    public interface IConfigurationItemEditorViewModelFactory:IConfigurationItemVisitor<IEditorConfigurationItemViewModel>
    {
      
    }
}