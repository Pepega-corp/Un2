using System;
using System.Collections.Generic;
using System.Windows;

namespace Unicon2.Fragments.Programming.Model
{
    [Serializable]
    public class ConnectionSegment
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public ConnectionSegment PreviousSegment { get; set; }
        public List<ConnectionSegment> NextSegments { get; }

        public ConnectionSegment(Point point1, Point point2, ConnectionSegment previousSegment)
        {
            Point1 = point1;
            Point2 = point2;
            PreviousSegment = previousSegment;
            NextSegments = new List<ConnectionSegment>();
        }
    }
}