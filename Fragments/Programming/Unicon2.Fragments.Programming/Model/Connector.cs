using System.Runtime.Serialization;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Namespace = "ConnectorNS")]
    public class Connector: IConnector
    {
        private IConnection _connection;
        /// <summary>
        /// Ориентация вывода по отношению к элементу: расположение справа или слева
        /// </summary>
        [DataMember] public ConnectorOrientation Orientation { get; }
        /// <summary>
        /// Тип вывода: прямой или инверсный
        /// </summary>
        [DataMember] public ConnectorType Type { get; set; }
        [DataMember] public Point ConnectorPoint { get; set; }
        /// <summary>
        /// Номер связи
        /// </summary>
        [DataMember] public int ConnectionNumber { get; set; }

        public Connector(ConnectorOrientation orientation, ConnectorType type)
        {
            this.Orientation = orientation;
            this.Type = type;
            this.ConnectionNumber = -1;
        }
    }
}
