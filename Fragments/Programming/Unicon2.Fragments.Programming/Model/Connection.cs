using Newtonsoft.Json;
using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Connection
    {
        [JsonProperty] public int ConnectionNumber { get; set; }
        [JsonProperty] public List<ConnectionSegment> Segments { get; set; } = new List<ConnectionSegment>();

        public Connection(List<SegmentPoint> pathPoints, int connectionNumber)
        {
            ConnectionNumber = connectionNumber;

            for (var i = 0; i < pathPoints.Count - 1; i++)
            {
                if (i == 0)
                {
                    var segment = new ConnectionSegment(pathPoints[i], pathPoints[i + 1], null);
                    Segments.Add(segment);
                }
                else
                {
                    var prevSegment = Segments[i - 1];
                    var segment = new ConnectionSegment(prevSegment.Point2, pathPoints[i + 1], prevSegment);
                    Segments.Add(segment);
                    prevSegment.NextSegments.Add(segment);
                }
            }
        }
    }
}