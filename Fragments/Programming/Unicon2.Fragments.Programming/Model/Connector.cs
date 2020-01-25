using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    public class Connector: IConnector
    {
        /// <summary>
        /// Ориентация вывода по отношению к элементу: расположение справа или слева
        /// </summary>
        public ConnectorOrientation Orientation { get; }
        /// <summary>
        /// Тип вывода: прямой или инверсный
        /// </summary>
        public ConnectorType Type { get; set; }
        /// <summary>
        /// Номер связи
        /// </summary>
        public int ConnectionNumber { get; set; }

        public Connector(ConnectorOrientation orientation, ConnectorType type)
        {
            this.Orientation = orientation;
            this.Type = type;
            this.ConnectionNumber = -1;
        }
    }
}
