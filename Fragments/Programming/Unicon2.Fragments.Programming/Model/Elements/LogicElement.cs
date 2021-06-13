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

        public abstract void BinProgramToProperty(ushort[] bin);
        public virtual void CopyLibraryValues(ILibraryElement source) { }

        public virtual void CopyValues(ILogicElement source)
        {
            Connectors = new List<IConnector>();
            foreach (var c in source.Connectors)
            {
                Connectors.Add(new Connector(c.Orientation, c.Type) { ConnectorPosition = c.ConnectorPosition });
            }
            X = source.X;
            Y = source.Y;
        }

        public abstract ushort[] GetProgramBin();
    }
}
