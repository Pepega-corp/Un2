using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Name = "schemeModel", Namespace ="SchemeModelNS")]
    public class SchemeModel : ISchemeModel
    {
        public SchemeModel(string name, double height, double width)
        {
            SchemeName = name;
            SchemeHeight = height;
            SchemeWidth = width;

            LogicElements = new ILogicElement[0];
        }
        [DataMember]
        public string SchemeName { get; set; }
        [DataMember]
        public double SchemeHeight { get; }
        [DataMember]
        public double SchemeWidth { get; }
        [DataMember]
        public double Scale { get; set; }
        [DataMember]
        public ILogicElement[] LogicElements { get ; set; }
        //IConnection[] here
    }
}
