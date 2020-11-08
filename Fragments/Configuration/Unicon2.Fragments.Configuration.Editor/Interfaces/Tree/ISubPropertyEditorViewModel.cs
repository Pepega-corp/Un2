using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface ISubPropertyEditorViewModel : IPropertyEditorViewModel
    {
        ObservableCollection<ISharedBitViewModel> BitNumbersInWord { get; set; }
        void SetMainBitNumbersInWord(ObservableCollection<ISharedBitViewModel> mainBitViewModels);

    }
}