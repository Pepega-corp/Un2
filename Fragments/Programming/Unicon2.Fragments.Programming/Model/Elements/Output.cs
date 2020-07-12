using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Output : LogicElement, IOutput
    {
        private const int BIN_SIZE = 3;
        
        [JsonProperty]
        public List<string> OutputSignals { get; set; }
        [JsonProperty]
        public int OutputSignalNum { get; set; }
        public override ElementType ElementType => ElementType.Out;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.INPUT_OUTPUT;
        public override int BinSize => BIN_SIZE;
        public override string StrongName => ProgrammingKeys.OUTPUT;

        public Output()
        {
            this.OutputSignals = new List<string>();
            this.Connectors = new List<IConnector> { new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT)};
        }

        public override void CopyValues(ILogicElement source)
        {
            if (!(source is Output outputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Output));
            }
            
            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(outputSource.OutputSignals);
            this.OutputSignalNum = outputSource.OutputSignalNum;

            base.CopyValues(source);
        }

        public override void CopyValues(ILibraryElement source)
        {
            if (!(source is IOutputEditor outputEditor))
            {
                throw new ArgumentException("Copied source is not " + typeof(IOutputEditor));
            }

            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(outputEditor.OutputSignals);
        }

        public override ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            bindata[0] = 5;
            bindata[1] = (ushort)this.OutputSignalNum;
            bindata[2] = (ushort)this.Connectors.First().ConnectionNumber;
            return bindata;
        }

        public override void BinProgrammToProperty(ushort[] bin)
        {
            this.OutputSignalNum = bin[1];
            this.Connectors[0].ConnectionNumber = bin[2];
        }
    }
}
