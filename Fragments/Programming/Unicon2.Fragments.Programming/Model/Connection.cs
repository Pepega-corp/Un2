using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Connection
    {
        [JsonProperty] public int ConnectionNumber { get; set; }
        [JsonProperty] public List<ConnectionSegment> Segments { get; set; }

        public Connection(List<Point> pathPoints, int connectionNumber)
        {
            ConnectionNumber = connectionNumber;

            if (pathPoints.Count % 2 != 0)
            {
                pathPoints.Add(new Point());
            }

            ConnectionSegment prevSegment = null;
            for (var i = 0; i < pathPoints.Count; i += 2)
            {
                
            }
        }
    }
}
