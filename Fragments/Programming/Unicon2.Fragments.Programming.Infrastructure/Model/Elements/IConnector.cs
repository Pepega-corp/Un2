using System.Windows;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnector
    {
        ConnectorOrientation Orientation { get; }
        ConnectorType Type { get; set; }
        Point ConnectorPoint { get; set; }
        IConnection Connection { get; set; }
        int ConnectionNumber { get; set; }
    }
}