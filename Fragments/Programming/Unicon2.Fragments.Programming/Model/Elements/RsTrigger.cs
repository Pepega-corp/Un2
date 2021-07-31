using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;  
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RsTrigger : LogicElement
    {
        public override ElementType ElementType => ElementType.RS;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.EXTENDET_LOGIC;
        public override int BinSize => 5;
        public override string StrongName => ProgrammingKeys.RS_TRIGGER;

        public RsTrigger()
        {
            Connectors = new List<Connector>
            {
                new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT),
                new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT),
                new Connector(ConnectorOrientation.RIGHT, ConnectorType.DIRECT)
            };
        }

        public override void CopyValues(LogicElement source)
        {
            if (source is RsTrigger)
            {
                base.CopyValues(source);
            }
            else
            {
                throw new Exception($"Source {source} is not And type");
            }
        }

        public override ushort[] GetProgramBin()
        {
            var bindata = new ushort[this.BinSize];
            bindata[0] = 11;
            var output = Connectors.First(c => c.Orientation == ConnectorOrientation.RIGHT);
            bindata[1] = (ushort)output.ConnectionNumber;
            var inputs = Connectors.Where(c => c.Orientation == ConnectorOrientation.LEFT).ToList();
            bindata[2] = (ushort)inputs[1].ConnectionNumber;
            bindata[3] = (ushort)inputs[0].ConnectionNumber;
            bindata[4] = 0;
            return bindata;
        }

        public override void BinProgramToProperty(ushort[] bin)
        {

        }
    }
}
