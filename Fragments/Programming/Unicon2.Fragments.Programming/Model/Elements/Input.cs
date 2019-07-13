using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    public class Input : IInput
    {
        private const int BIN_SIZE = 3;

        private ushort _inputSignal;
        private ushort _base;
        private int _connectionNumber;

        public Dictionary<int, Dictionary<int, string>> AllInputSignals { get; }
        public List<string> Bases { get; }

        public Input()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;
            this.AllInputSignals = new Dictionary<int, Dictionary<int, string>>();
            this.Bases = new List<string>();
        }

        private Input(Input cloneable)
        {
            this.Functional = cloneable.Functional;
            this.Group = cloneable.Group;
            this._inputSignal = cloneable._inputSignal;
            this._base = cloneable._base;
            this._connectionNumber = cloneable._connectionNumber;
            this.Bases = new List<string>(cloneable.Bases);

            this.AllInputSignals = new Dictionary<int, Dictionary<int, string>>(cloneable.AllInputSignals);
            for (int i = 0; i < this.Bases.Count; i++)
            {
                var copiedDictionary = cloneable.AllInputSignals[i];
                this.AllInputSignals[i] = new Dictionary<int, string>(copiedDictionary);
            }
        }

        public Functional Functional { get; }
        public Group Group { get; }
        public int BinSize => BIN_SIZE;

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
            bindata[2] = (ushort)this._connectionNumber;
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this._base = bin[0];
            this._inputSignal = bin[1];
            this._connectionNumber = bin[2];
        }

        public int ConnectionNumber
        {
            get { return this._connectionNumber; }
            set { this._connectionNumber = value; }
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
