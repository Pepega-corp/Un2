using System.Collections.Generic;
using System.Windows;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IConnection
    {
        int ConnectionNumber { get; set; }
        List<Point> Points { get; set; }
    }
}