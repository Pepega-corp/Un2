using System.Collections.ObjectModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface ICollectionValueViewModel : IFormattedValueViewModel
    {
        ObservableCollection<IFormattedValueViewModel> ValuesObservableCollection { get; set; }
    }
}