using System.Collections.ObjectModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel
{
    public interface IRuntimeConfigurationItemViewModel : IConfigurationItemViewModel
    {
        ObservableCollection<IRuntimeConfigurationItemViewModel> ChildStructItemViewModels { get; }
        IRuntimeConfigurationItemViewModel Parent { get; set; }
    }
}
