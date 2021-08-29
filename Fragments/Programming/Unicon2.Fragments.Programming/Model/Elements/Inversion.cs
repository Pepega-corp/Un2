using System;
using System.Collections.Generic;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    class Inversion : LogicElement
    {
        public override ElementType ElementType => ElementType.Inversion;
        public override string Name => this.ElementType.ToString("G");
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.SIMPLE_LOGIC;
        public override int BinSize => 3;

        public Inversion()
        {
            Connectors = new List<Connector>
            {
                new Connector(new Point(), ConnectorOrientation.LEFT, ConnectorType.DIRECT),
                new Connector(new Point(), ConnectorOrientation.RIGHT, ConnectorType.INVERS)
            };
        }
        
        public override void CopyValues(LogicElement source)
        {
            if (source is Inversion)
            {
                base.CopyValues(source);
            }
            else
            {
                throw new Exception($"Source {source} is not Inversion type");
            }
        }

        public override ushort[] GetProgramBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            bindata[0] = 9;
            bindata[1] = (ushort)Connectors[1].ConnectionNumber;// Out
            bindata[1] |= 0x8000;
            bindata[2] = (ushort)Connectors[0].ConnectionNumber;// In
            return bindata;
        }

        public override void BinProgramToProperty(ushort[] bin) { }

        public override string StrongName => ProgrammingKeys.INVERSION;
    }
}
