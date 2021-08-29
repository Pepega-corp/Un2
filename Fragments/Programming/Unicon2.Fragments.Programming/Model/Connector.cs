using Newtonsoft.Json;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Connector : SegmentPoint
    {
        /// <summary>
        /// Ориентация вывода по отношению к элементу: расположение справа или слева
        /// </summary>
        [JsonProperty] public ConnectorOrientation Orientation { get; set; }
        /// <summary>
        /// Тип вывода: прямой или инверсный
        /// </summary>
        [JsonProperty] public ConnectorType Type { get; set; }
        /// <summary>
        /// Номер связи
        /// </summary>
        [JsonProperty] public int ConnectionNumber { get; set; }

        public Connector(Connector source) : this(source.Position, source.Orientation, source.Type)
        {
            
        }

        public Connector(Point position, ConnectorOrientation orientation, ConnectorType type) : base(position)
        {
            this.Orientation = orientation;
            this.Type = type;
            this.ConnectionNumber = -1;
        }
    }
}
