using System;
using System.Collections.Generic;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [Serializable]
    public class ConnectionSegment : IConnectionSegment
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public IConnectionSegment PreviousSegment { get; set; }
        public List<IConnectionSegment> NextSegments { get; }

        public ConnectionSegment(Point point1, Point point2, IConnectionSegment previousSegment)
        {
            Point1 = point1;
            Point2 = point2;
            PreviousSegment = previousSegment;
            NextSegments = new List<IConnectionSegment>();
        }
    }
}