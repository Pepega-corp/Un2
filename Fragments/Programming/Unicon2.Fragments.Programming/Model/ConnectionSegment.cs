using System;
using System.Collections.Generic;
using System.Windows;

namespace Unicon2.Fragments.Programming.Model
{
    [Serializable]
    public class ConnectionSegment
    {
        public SegmentPoint Point1 { get; set; }
        public SegmentPoint Point2 { get; set; }
        public ConnectionSegment PreviousSegment { get; set; }
        public List<ConnectionSegment> NextSegments { get; }

        public ConnectionSegment(Point point1, Point point2, ConnectionSegment previousSegment) 
            : this(new SegmentPoint(point1), new SegmentPoint(point2), previousSegment)
        {
           
        }

        public ConnectionSegment(SegmentPoint point1, SegmentPoint point2, ConnectionSegment previousSegment)
        {
            Point1 = point1;
            Point2 = point2;
            PreviousSegment = previousSegment;
            NextSegments = new List<ConnectionSegment>();
        }
    }
}