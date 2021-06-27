using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Connection : IConnection
    {
        [JsonProperty] public int ConnectionNumber { get; set; }
        [JsonProperty] public List<IConnectionSegment> Segments { get; set; }

        public Connection(List<Point> pathPoints, int connectionNumber)
        {
            ConnectionNumber = connectionNumber;

            if (pathPoints.Count % 2 != 0)
            {
                pathPoints.Add(new Point());
            }

            IConnectionSegment prevSegment = null;
            for (var i = 0; i < pathPoints.Count; i += 2)
            {
                
            }
        }
    }
}
