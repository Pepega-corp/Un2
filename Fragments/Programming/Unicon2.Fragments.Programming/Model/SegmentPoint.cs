using System;
using System.Windows;

namespace Unicon2.Fragments.Programming.Model
{
    [Serializable]
    public class SegmentPoint
    {
        [NonSerialized] protected ConnectionSegment _connectionSegment;
        
        public Point Position { get; set; }
        public virtual ConnectionSegment ConnectionSegment
        {
            get => _connectionSegment;
            set => _connectionSegment = value;
        }

        public SegmentPoint()
        {
            Position = new Point();
        }

        public SegmentPoint(Point position)
        {
            this.Position = position;
        }
    }
}