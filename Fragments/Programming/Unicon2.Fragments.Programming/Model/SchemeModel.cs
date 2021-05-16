using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SchemeModel : ISchemeModel
    {
        public SchemeModel(string name, Size schemeSize)
        {
            this.SchemeName = name;
            this.SchemeHeight = schemeSize.Height;
            this.SchemeWidth = schemeSize.Width;
            this.LogicElements = new List<ILogicElement>();
            this.ConnectionNumbers = new List<int>();
        }
        [JsonProperty]
        public string SchemeName { get; set; }
        [JsonProperty]
        public double SchemeHeight { get; set; }
        [JsonProperty]
        public double SchemeWidth { get; set; }
        [JsonProperty]
        public List<ILogicElement> LogicElements { get ; set; }
        [JsonProperty]
        public List<int> ConnectionNumbers { get; set; }
    }
}
