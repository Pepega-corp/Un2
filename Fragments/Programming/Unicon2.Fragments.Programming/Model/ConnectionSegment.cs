using System;
using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Model
{
    [Serializable]
    public class ConnectionSegment
    {
        [NonSerialized] private Connection _connection;
        
        public SegmentPoint Point1 { get; set; }
        public SegmentPoint Point2 { get; set; }
        public ConnectionSegment PreviousSegment { get; set; }
        public List<ConnectionSegment> NextSegments { get; }
        public Connection Connection
        {
            get => _connection;
            set => _connection = value;
        }

        public ConnectionSegment(Connection connection, SegmentPoint point1, SegmentPoint point2, ConnectionSegment previousSegment)
        {
            _connection = connection;
            Point1 = point1;
            Point1.ConnectionSegment = this;
            Point2 = point2;
            Point2.ConnectionSegment = this;
            PreviousSegment = previousSegment;
            NextSegments = new List<ConnectionSegment>();
        }
    }
}