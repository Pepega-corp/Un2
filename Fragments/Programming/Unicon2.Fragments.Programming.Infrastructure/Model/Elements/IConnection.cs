namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnection
    {
        int ConnectionNumber { get; set; }
        IConnector[] Connectors { get; }
        void AddConnector(IConnector connector);
        void RemoveConnector(IConnector connector);
    }
}