using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.EditOperations;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface ISubPropertyEditorViewModel : IEditorConfigurationItemViewModel, IEditable
    {
        ObservableCollection<ISharedBitViewModel> BitNumbersInWord { get; set; }
        void SetMainBitNumbersInWord(ObservableCollection<ISharedBitViewModel> mainBitViewModels);

    }
}