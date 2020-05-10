using System.Runtime.Serialization;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Name = "schemeModel", Namespace ="SchemeModelNS")]
    public class SchemeModel : ISchemeModel
    {
        public SchemeModel(string name, Size schemeSize)
        {
            this.SchemeName = name;
            this.SchemeHeight = schemeSize.Height;
            this.SchemeWidth = schemeSize.Width;
            this.Scale = 1;
            this.LogicElements = new ILogicElement[0];
        }
        [DataMember]
        public string SchemeName { get; set; }
        [DataMember]
        public double SchemeHeight { get; set; }
        [DataMember]
        public double SchemeWidth { get; set; }
        [DataMember]
        public double Scale { get; set; }
        [DataMember]
        public ILogicElement[] LogicElements { get ; set; }
        [DataMember]
        public int[] ConnectionNumbers { get; set; }
    }
}
