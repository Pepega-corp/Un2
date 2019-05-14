using System;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    public class Input : ILogicElement
    {
        public const string INP_SIGNAL = "InpSignal";
        public const string BASE = "Base";
        public const string CONNECTION_NUMER = "ConnectionNumber";
        public const string LIST_OF_SIGNALS = "InpList";
        private const int BIN_SIZE = 3;
        
        public Input()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;
            this.Property = new Dictionary<string, object>
            {
                {INP_SIGNAL, 0},
                {BASE, 0},
                {CONNECTION_NUMER, new ushort?()},
                {LIST_OF_SIGNALS, new Dictionary<int, string>()}
            };
        }

        private Input(Input cloneable)
        {
            this.Functional = cloneable.Functional;
            this.Group = cloneable.Group;
            this.Property = new Dictionary<string, object>(cloneable.Property);
        }

        public Functional Functional { get; }
        public Group Group { get; }
        public int BinSize => BIN_SIZE;

        public ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            switch (Convert.ToInt32(this.Property[BASE]))
            {
                case 0:
                {
                    bindata[0] = 2;
                    break;
                }
                case 1:
                {
                    bindata[0] = 3;
                    break;
                }
                case 2:
                {
                    bindata[0] = 4;
                    break;
                }
                case 3:
                {
                    bindata[0] = 33;
                    break;
                }
                case 4:
                {
                    bindata[0] = 34;
                    break;
                }
            }
            bindata[1] = Convert.ToUInt16(this.Property[INP_SIGNAL]);
            bindata[2] = Convert.ToUInt16(this.Property[CONNECTION_NUMER]);
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this.Property[BASE] = bin[0];
            this.Property[INP_SIGNAL] = bin[1];
            this.Property[CONNECTION_NUMER] = bin[2];
        }

        public Dictionary<string, object> Property { get; }

        #region IStronglyName
        public string StrongName => ProgrammingKeys.INPUT;
        #endregion

        public object Clone()
        {
            return new Input(this);
        }
    }
}
