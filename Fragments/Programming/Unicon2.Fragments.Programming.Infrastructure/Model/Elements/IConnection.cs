using System.Windows.Media;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnection
    {
        int ConnectionNumber { get; set; }
        PathGeometry Path { get; set; }
        //IConnector[] Connectors { get; }
        //void AddConnector(IConnector connector);
        //void RemoveConnector(IConnector connector);
        //void Clear();
    }
}