using System.Collections.ObjectModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Dependencies
{
    public interface IDependenciesViewModelContainer
    {
        ObservableCollection<IDependencyViewModel> DependencyViewModels { get; }
    }
}