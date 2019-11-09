using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IPropertyEditorEditorViewModel : IPropertyViewModel, IEditable, IDeletable, IAddressIncreaseableDecreaseable,
        IUshortFormattableEditorViewModel
    {
        string Address { get; set; }
        string NumberOfPoints { get; set; }
    }
}