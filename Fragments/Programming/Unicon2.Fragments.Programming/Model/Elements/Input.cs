using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [DataContract(Namespace = "InputNS")]
    public class Input : IInput
    {
        private const int BIN_SIZE = 3;
        private ushort _inputSignal;
        private ushort _base;

        [DataMember]
        public List<Dictionary<int, string>> AllInputSignals { get; set; }
        [DataMember]
        public List<string> Bases { get; set; }
        [DataMember]
        public int ConnectionNumber { get; set; }

        public Input()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;

            this.Bases = new List<string> {"Base0"};

            this.AllInputSignals =
                new List<Dictionary<int, string>>
                {
                    new Dictionary<int, string> {{0, string.Empty}}
                };
        }

        private Input(Input cloneable)
        {
            this.CopyValues(cloneable);
        }

        public Functional Functional { get; private set; }
        public Group Group { get; private set; }
        public int BinSize => BIN_SIZE;

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Input inputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Input));
            }

            this.Functional = inputSource.Functional;
            this.Group = inputSource.Group;
            this._inputSignal = inputSource._inputSignal;
            this._base = inputSource._base;
            this.ConnectionNumber = inputSource.ConnectionNumber;
            this.Bases = new List<string>(inputSource.Bases);

            this.AllInputSignals = new List<Dictionary<int, string>>(inputSource.AllInputSignals);
            for (int i = 0; i < this.Bases.Count; i++)
            {
                var copiedDictionary = inputSource.AllInputSignals[i];
                this.AllInputSignals[i] = new Dictionary<int, string>(copiedDictionary);
            }
        }

        public ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            switch (this._base)
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
            bindata[1] = this._inputSignal;
            bindata[2] = (ushort)this.ConnectionNumber;
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this._base = bin[0];
            this._inputSignal = bin[1];
            this.ConnectionNumber = bin[2];
        }

        #region IStronglyName
        public string StrongName => ProgrammingKeys.INPUT;
        #endregion

        public object Clone()
        {
            return new Input(this);
        }
    }
}
