using System.Collections.ObjectModel;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface ILogicElementViewModel : ISchemeElement, IViewModel
    {
        string ElementName { get; }
        string Caption { get; set; }
        string Description { get; }
        string Symbol { get; }
        ObservableCollection<IConnectorViewModel> Connectors { get; }
        object Clone();
        void OpenPropertyWindow();
    }
}