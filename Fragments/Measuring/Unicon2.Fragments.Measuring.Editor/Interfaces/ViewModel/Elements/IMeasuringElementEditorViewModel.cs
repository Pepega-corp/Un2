using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements
{
    public interface IMeasuringElementEditorViewModel : IUniqueIdWithSet
    {
        string Header { get; set; }
        string NameForUiKey { get; }
         ObservableCollection<IDependencyViewModel> DependencyViewModels
        { get; }
    }
}