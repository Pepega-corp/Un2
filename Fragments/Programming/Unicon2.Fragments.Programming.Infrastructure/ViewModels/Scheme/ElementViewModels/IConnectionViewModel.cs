using System;
using System.Windows;
using System.Windows.Media;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface IConnectionViewModel : ILogicElementViewModel
    {
        event Action<IConnectionViewModel> DeleteConnection;
        void OnDeleteConnection(IConnectionViewModel connection);

        PathGeometry Path { get; set; }
        IConnectorViewModel Source { get; set; }
        IConnectorViewModel Sink { get; set; }
        int ConnectionNumber { get; }
        ushort CurrentValue { get; set; }
        bool GotValue { get; set; }
        Point LabelPosition { get; set; }
        DoubleCollection StrokeDashArray { get; set; }
    }
}