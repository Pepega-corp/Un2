using System;
using System.Windows;
using System.Windows.Media;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface IConnectionViewModel
    {
        void UpdateConnector(IConnectorViewModel connector);
        string Name { get; }
        PathGeometry Path { get; set; }
        IConnectorViewModel Source { get; set; }
        IConnectorViewModel Sink { get; set; }
        int ConnectionNumber { get; }
        ushort CurrentValue { get; set; }
        bool GotValue { get; set; }
        Point LabelPosition { get; set; }
        DoubleCollection StrokeDashArray { get; set; }
        bool IsSelected { get; set; }
        double Value { get; }
    }
}