using Newtonsoft.Json;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class LogicElement : ILogicElement
    {
        public abstract ElementType ElementType { get; }
        public abstract string Name { get; }
        public abstract Functional Functional { get; }
        public abstract Group Group { get; }
        public abstract int BinSize { get; }
        public abstract string StrongName { get; }
        [JsonProperty]
        public List<IConnector> Connectors { get; set; }
        [JsonProperty]
        public double X { get; set; }
        [JsonProperty]
        public double Y { get; set; }

        public abstract void BinProgrammToProperty(ushort[] bin);
        public virtual void CopyLibraryValues(ILibraryElement source) { }
        public abstract void CopyValues(ILogicElement source);
        public abstract ushort[] GetProgrammBin();
    }
}
