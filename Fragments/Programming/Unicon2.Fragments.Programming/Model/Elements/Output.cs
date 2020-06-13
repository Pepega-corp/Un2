using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [DataContract(Namespace = "OutputNS")]
    public class Output : IOutput
    {
        private const int BIN_SIZE = 3;

        [DataMember]
        public IConnector[] Connectors { get; set; }
        [DataMember]
        public List<string> OutputSignals { get; set; }
        [DataMember]
        public int OutputSignalNum { get; set; }
        [DataMember]
        public double X { get; set; }
        [DataMember]
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

        private Output(Output cloneable):this()
        {
            this.CopyValues(cloneable);
        }

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Output outputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Output));
            }
            
            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(outputSource.OutputSignals);

            //this.Connectors = new IConnector[outputSource.Connectors.Length];
            //for (int i = 0; i < outputSource.Connectors.Length; i++)
            //{
            //    var connector = outputSource.Connectors[i];
            //    this.Connectors[i] = new Connector(connector.Orientation, connector.Type);
            //    this.Connectors[i].ConnectionNumber = connector.ConnectionNumber;
            //}
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
