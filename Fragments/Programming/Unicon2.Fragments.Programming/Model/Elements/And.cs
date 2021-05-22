using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class And : LogicElement, IAnd
    {
        public override ElementType ElementType => ElementType.And;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.SIMPLE_LOGIC;
        public override int BinSize => Connectors.Count(c=>c.Orientation == ConnectorOrientation.LEFT) + 2;
        public override string StrongName => ProgrammingKeys.AND;

        public And()
        {
            Connectors = new List<IConnector>
            {
                new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT),
                new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT),
                new Connector(ConnectorOrientation.RIGHT, ConnectorType.DIRECT)
            };
        }

        public override void CopyValues(ILogicElement source)
        {
            if (source is And)
            {
                base.CopyValues(source);
            }
            else
            {
                throw new Exception($"Source {source} is not And type");
            }
        }

        public override ushort[] GetProgrammBin()
        {
            var inputs = Connectors.Where(c => c.Orientation == ConnectorOrientation.LEFT).ToArray();
            var output = Connectors.First(c => c.Orientation == ConnectorOrientation.RIGHT);
            var inputsCount = inputs.Count();
            ushort[] bindata = new ushort[BinSize];
            bindata[0] = (ushort)(6 + inputsCount * 0x0100);
            bindata[1] = (ushort)output.ConnectionNumber;
            for (int i = 0; i < inputsCount; i++)
            {
                bindata[2 + i] = (ushort)inputs[i].ConnectionNumber;
                if (inputs[i].Type == ConnectorType.INVERS) 
                { 
                    bindata[1 + i] |= 0x8000; 
                }
            }
            return bindata;
        }

        public override void BinProgrammToProperty(ushort[] bin)
        { 

        }
    }
}
