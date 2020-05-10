using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnection
    {
        int ConnectionNumber { get; set; }
        List<Point> Points { get; set; }
        PathGeometry Path { get; set; }
    }
}