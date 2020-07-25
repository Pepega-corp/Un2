using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface ILogicElementViewModel : ISchemeElementViewModel, IViewModel<ILogicElement>
    {
        string ElementName { get; }
        string Caption { get; set; }
        string Description { get; }
        string Symbol { get; }
        bool Connected { get; }
        ObservableCollection<IConnectorViewModel> ConnectorViewModels { get; }
        ILogicElementViewModel Clone();
        void OpenPropertyWindow();
    }
}