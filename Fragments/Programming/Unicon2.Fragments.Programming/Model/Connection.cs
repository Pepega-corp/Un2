using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Connection : IConnection
    {
        [JsonProperty] public int ConnectionNumber { get; set; }
        [JsonProperty] public List<Point> Points { get; set; }
    }
}
