using System.Collections.ObjectModel;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface IConnectorViewModel
    {
        string Symbol {get;}

        Point ConnectorPosition { get; set; }
        ILogicElementViewModel ParentViewModel { get; }
        ObservableCollection<IConnectionViewModel> Connections { get; }
        IConnector Model { get; set; }
        ConnectorType ConnectorType { get; set; }
        ConnectorOrientation Orientation { get; }
        bool Connected { get; }

        void UpdateConnectorPositionX(double deltaX);
        void UpdateConnectorPositionY(double deltaY);
    }
}