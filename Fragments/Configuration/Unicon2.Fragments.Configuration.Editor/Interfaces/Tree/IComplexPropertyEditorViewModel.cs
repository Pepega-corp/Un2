using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.EditOperations;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IComplexPropertyEditorViewModel : IChildPositionChangeable, ISubPropertyAddable,
        IChildItemRemovable, IPropertyEditorViewModel
    {
        ObservableCollection<ISubPropertyEditorViewModel> SubPropertyEditorViewModels { get; set; }
        ObservableCollection<ISharedBitViewModel> MainBitNumbersInWordCollection { get; set; }
        bool IsGroupedProperty { get; set; }
        ICommand SubmitCommand { get; set; }
        ICommand CancelCommand { get; set; }

    }
}