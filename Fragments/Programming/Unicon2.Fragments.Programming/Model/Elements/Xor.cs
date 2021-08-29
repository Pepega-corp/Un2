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
    public class Xor : LogicElement
    {
        public override ElementType ElementType => ElementType.Xor;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.SIMPLE_LOGIC;
        public override int BinSize => Connectors.Count(c => c.Orientation == ConnectorOrientation.LEFT) + 2;
        public override string StrongName => ProgrammingKeys.XOR;

        public Xor()
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
            if (source is Xor)
            {
                base.CopyValues(source);
            }
            else
            {
                throw new Exception($"Source {source} is not Xor type");
            }
        }

        public override ushort[] GetProgramBin()
        {
            var inputs = Connectors.Where(c => c.Orientation == ConnectorOrientation.LEFT).ToArray();
            var output = Connectors.First(c => c.Orientation == ConnectorOrientation.RIGHT);
            var inputsCount = inputs.Count();
            var bindata = new ushort[BinSize];
            bindata[0] = (ushort)(8 + inputsCount * 0x0100);
            bindata[1] = (ushort)output.ConnectionNumber;
            for (var i = 0; i < inputsCount; i++)
            {
                bindata[2 + i] = (ushort)inputs[i].ConnectionNumber;
                if (inputs[i].Type == ConnectorType.INVERS)
                {
                    bindata[1 + i] |= 0x8000;
                }
            }
            return bindata;
        }

        public override void BinProgramToProperty(ushort[] bin)
        {

        }
    }
}