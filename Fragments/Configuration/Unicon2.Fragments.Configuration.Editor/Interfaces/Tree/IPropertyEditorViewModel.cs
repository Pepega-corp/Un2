using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IPropertyEditorViewModel : IEditorConfigurationItemViewModel, IPropertyViewModel, IEditable,
        IDeletable, IAddressChangeable,
        IUshortFormattableEditorViewModel,IDependenciesViewModelContainer, IBitsConfigViewModel
    {
        string Address { get; set; }
        string NumberOfPoints { get; set; }
        ushort NumberOfWriteFunction { get; set; }
    }


}