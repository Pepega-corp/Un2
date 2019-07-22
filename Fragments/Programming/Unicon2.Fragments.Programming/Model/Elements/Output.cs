using System;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    public class Output : ILogicElement
    {
        public const string OUT_SIGNAL = "OutSignal";
        public const string CONNECTION_NUMER = "ConnectionNumber";
        public const string LIST_OF_SIGNALS = "OutList";
        private const int BIN_SIZE = 3;

        public Output()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;
            this.Property = new Dictionary<string, object>
            {
                {OUT_SIGNAL, 0},
                {CONNECTION_NUMER, new ushort?()}
            };
        }

        private Output(Output cloneable)
        {
            CopyValues(cloneable);
        }

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Output outputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Output));
            }

            this.Functional = outputSource.Functional;
            this.Group = outputSource.Group;
            this.Property = new Dictionary<string, object>(outputSource.Property);
        }

        public Functional Functional { get; private set; }
        public Group Group { get; private set;}
        public int BinSize => BIN_SIZE;

        public ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            bindata[0] = 5;
            bindata[1] = Convert.ToUInt16(this.Property[OUT_SIGNAL]);
            bindata[2] = Convert.ToUInt16(this.Property[CONNECTION_NUMER]);
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this.Property[OUT_SIGNAL] = bin[1];
            this.Property[CONNECTION_NUMER] = bin[2];
        }

        public Dictionary<string, object> Property { get; private set; }

        #region IStronglyName
        public string StrongName => ProgrammingKeys.OUTPUT;
        #endregion

        public object Clone()
        {
            return new Output(this);
        }
    }
}
