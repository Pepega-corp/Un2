using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Name = "connection",Namespace = "ConnectionNS")]
    public class Connection : IConnection
    {
        [DataMember] public int ConnectionNumber { get; set; }
        [DataMember] public List<Point> Points { get; set; }
        /*[DataMember]*/ public PathGeometry Path { get; set; }
    }
}
