using System.Collections.Generic;
using System.Windows;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnectionSegment
    {
        Point Point1 { get; set; }
        Point Point2 { get; set; }
        IConnectionSegment PreviousSegment { get; set; }
        List<IConnectionSegment> NextSegments { get; }
    }
}