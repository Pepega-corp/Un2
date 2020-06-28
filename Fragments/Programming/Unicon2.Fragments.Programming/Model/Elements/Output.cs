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
    public class Output : IOutput
    {
        private const int BIN_SIZE = 3;

        [JsonProperty]
        public IConnector[] Connectors { get; set; }
        [JsonProperty]
        public List<string> OutputSignals { get; set; }
        [JsonProperty]
        public int OutputSignalNum { get; set; }
        [JsonProperty]
        public double X { get; set; }
        [JsonProperty]
        public double Y { get; set; }
        public ElementType ElementType => ElementType.Out;
        public string Name => this.ElementType.ToString();
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.INPUT_OUTPUT;
        public int BinSize => BIN_SIZE;

        public Output()
        {
            this.OutputSignals = new List<string>();
            this.Connectors = new IConnector[] {new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT)};
        }

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Output outputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Output));
            }
            
            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(outputSource.OutputSignals);
        }

        public void CopyValues(ILibraryElement source)
        {
            if (!(source is IOutputEditor outputEditor))
            {
                throw new ArgumentException("Copied source is not " + typeof(IOutputEditor));
            }

            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(outputEditor.OutputSignals);
        }

        public ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            bindata[0] = 5;
            bindata[1] = (ushort)this.OutputSignalNum;
            bindata[2] = (ushort)this.Connectors.First().ConnectionNumber;
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this.OutputSignalNum = bin[1];
            this.Connectors[0].ConnectionNumber = bin[2];
        }

        #region IStronglyName
        public string StrongName => ProgrammingKeys.OUTPUT;
        #endregion
    }
}
