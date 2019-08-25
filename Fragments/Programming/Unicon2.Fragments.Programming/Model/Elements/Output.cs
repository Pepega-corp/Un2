using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [DataContract(Namespace = "OutputNS")]
    public class Output : IOutput
    {
        private const int BIN_SIZE = 3;
        private const int DEFAULT_SIZE = 32;

        [DataMember]
        public List<string> OutputSignals { get; set; }
        [DataMember]
        public int OutputSignalNum { get; set; }
        [DataMember]
        public int ConnectionNumber { get; set; }

        public Output()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;

            this.OutputSignals = new List<string>();
            for (int i = 0; i < DEFAULT_SIZE; i++)
            {
                this.OutputSignals.Add($"ССЛ{i + 1}");
            }
        }

        private Output(Output cloneable)
        {
            this.OutputSignals = new List<string>();
            this.CopyValues(cloneable);
        }

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Output outputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Output));
            }

            this.Functional = outputSource.Functional;
            this.Group = outputSource.Group;

            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(outputSource.OutputSignals);
        }

        public Functional Functional { get; private set; }
        public Group Group { get; private set; }
        public int BinSize => BIN_SIZE;

        public ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            bindata[0] = 5;
            bindata[1] = (ushort)this.OutputSignalNum;
            bindata[2] = (ushort)this.ConnectionNumber;
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this.OutputSignalNum = bin[1];
            this.ConnectionNumber = bin[2];
        }

        #region IStronglyName
        public string StrongName => ProgrammingKeys.OUTPUT;
        #endregion

        public object Clone()
        {
            return new Output(this);
        }
    }
}
