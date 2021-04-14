
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface IConnectionViewModel :IViewModel<IConnection>, ISchemeElementViewModel
    {
        event Action<IConnectionViewModel> NeedDelete;

        string Name { get; }
        PathGeometry Path { get; set; }
        int ConnectionNumber { get; }
        IConnectorViewModel SourceConnector { get; set; }
        ObservableCollection<IConnectorViewModel> SinkConnectors { get; }
        ushort CurrentValue { get; set; }
        bool GotValue { get; set; }
        bool DebugMode { get; set; }
        Point LabelPosition { get; set; }
        DoubleCollection StrokeDashArray { get; set; }
        double Value { get; }
        IConnectorViewModel GetNearConnector(IConnectorViewModel startConnector);
    }
}