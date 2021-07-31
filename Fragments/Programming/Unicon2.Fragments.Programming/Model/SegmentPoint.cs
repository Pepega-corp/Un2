using System;
using System.Windows;
using Newtonsoft.Json;

namespace Unicon2.Fragments.Programming.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SegmentPoint 
    {
        [JsonProperty] public Point Position { get; set; }

        public SegmentPoint(Point position)
        {
            this.Position = position;
        }
    }
}

