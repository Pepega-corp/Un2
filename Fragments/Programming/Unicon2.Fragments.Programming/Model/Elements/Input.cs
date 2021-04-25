using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Input : LogicElement, IInput
    {
        private const int BIN_SIZE = 3;

        [JsonProperty]
        public List<Dictionary<int, string>> AllInputSignals { get; set; }
        [JsonProperty]
        public int InputSignalNum { get; set; }
        [JsonProperty]
        public List<string> Bases { get; set; }
        [JsonProperty]
        public int BaseNum { get; set; }
        public override ElementType ElementType => ElementType.In;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.INPUT_OUTPUT;
        public override int BinSize => BIN_SIZE;
        public override string StrongName => ProgrammingKeys.INPUT;

        public Input()
        {
            this.Connectors = new List<IConnector> { new Connector(ConnectorOrientation.RIGHT, ConnectorType.DIRECT) };
            Bases = new List<string>();
            AllInputSignals = new List<Dictionary<int, string>>();
        }

        public override void CopyValues(ILogicElement source)
        {
            if (source is Input model)
            {
                this.Bases = new List<string>(model.Bases);
                this.AllInputSignals = new List<Dictionary<int, string>>(model.AllInputSignals);
                for (int i = 0; i < this.Bases.Count; i++)
                {
                    var copiedDictionary = model.AllInputSignals[i];
                    this.AllInputSignals[i] = new Dictionary<int, string>(copiedDictionary);
                }
                this.InputSignalNum = model.InputSignalNum;
                this.BaseNum = model.BaseNum;
                base.CopyValues(source);
            }
            else
            {
                throw new Exception($"Source {source} is not Input type");
            }
        }

        public override void CopyLibraryValues(ILibraryElement source)
        {
            if (!(source is IInputEditor inputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Input));
            }
            this.Bases.Clear();
            this.Bases.AddRange(inputSource.Bases);
            this.AllInputSignals = new List<Dictionary<int, string>>(inputSource.AllInputSignals);

            for (int i = 0; i < this.Bases.Count; i++)
            {
                var copiedDictionary = inputSource.AllInputSignals[i];
                this.AllInputSignals[i] = new Dictionary<int, string>(copiedDictionary);
            }
        }

        public override ushort[] GetProgrammBin()
        {
            ushort[] bindata = new ushort[this.BinSize];
            switch (this.BaseNum)
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
            bindata[1] = (ushort)this.InputSignalNum;
            bindata[2] = (ushort)this.Connectors.First().ConnectionNumber;
            return bindata;
        }

        public override void BinProgrammToProperty(ushort[] bin)
        {
            this.BaseNum = bin[0];
            this.InputSignalNum = bin[1];
            this.Connectors[0].ConnectionNumber = bin[2];
        }
    }
}
