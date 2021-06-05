using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Timer : LogicElement
    {
        private const int BIN_SIZE = 6;
        public override ElementType ElementType => ElementType.Timer;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.EXTENDET_LOGIC;
        public override int BinSize => BIN_SIZE;
        public override string StrongName => ProgrammingKeys.TIMER;

        public string[] TimerTypes { get; }

        [JsonProperty] public int SelectedTypeIndex { get; set; }
        [JsonProperty] public double Time { get; set; }

        public Timer()
        {
            this.Connectors = new List<IConnector>
            {
                new Connector(ConnectorOrientation.RIGHT, ConnectorType.DIRECT),
                new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT)
            };

            this.TimerTypes = new[]
            {
                "на сраб.",
                "на возвр.",
                "имп. по фр.1",
                "имп. по сп.1",
                "имп. по фр.2",
                "имп. по сп.2"
            };
            this.SelectedTypeIndex = 0;
            this.Time = 0.25;
        }

        public override void CopyValues(ILogicElement source)
        {
            if (source is Timer model)
            {
                this.SelectedTypeIndex = model.SelectedTypeIndex;
                this.Time = model.Time;
            }
            else
            {
                throw new Exception($"Source {source} is not Timer type");
            }
        }

        public override ushort[] GetProgramBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            bindata[0] = (ushort)(12 + this.SelectedTypeIndex * 0x0100);
            var output = Connectors.First(c => c.Orientation == ConnectorOrientation.RIGHT);
            bindata[1] = output.Type == ConnectorType.DIRECT
                ? (ushort) output.ConnectionNumber
                : (ushort) (output.ConnectionNumber | 0x8000);
            var input = Connectors.First(c => c.Orientation == ConnectorOrientation.LEFT);
            bindata[2] = input.Type == ConnectorType.DIRECT
                ? (ushort)input.ConnectionNumber
                : (ushort)(input.ConnectionNumber | 0x8000);
            bindata[3] = (ushort)(this.Time * 100);
            bindata[4] = 0;
            bindata[5] = 0;
            return bindata;
        }

        public override void BinProgramToProperty(ushort[] bin)
        {
        }
    }
}
