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
    public class Input : IInput
    {
        private const int BIN_SIZE = 3;

        [JsonProperty]
        public IConnector[] Connectors { get; set; }
        [JsonProperty]
        public List<Dictionary<int, string>> AllInputSignals { get; set; }
        [JsonProperty]
        public int InputSignalNum { get; set; }
        [JsonProperty]
        public List<string> Bases { get; set; }
        [JsonProperty]
        public int BaseNum { get; set; }
        [JsonProperty]
        public double X { get; set; }
        [JsonProperty]
        public double Y { get; set; }
        public ElementType ElementType => ElementType.In;
        public string Name => this.ElementType.ToString();
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.INPUT_OUTPUT;
        public int BinSize => BIN_SIZE;

        public Input()
        {
            this.Connectors = new IConnector[] { new Connector(ConnectorOrientation.RIGHT, ConnectorType.DIRECT) };
            this.Bases = new List<string> {"Base0"};

            this.AllInputSignals =
                new List<Dictionary<int, string>>
                {
                    new Dictionary<int, string> {{0, string.Empty}}
                };
        }

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Input inputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Input));
            }
            
            this.InputSignalNum = inputSource.InputSignalNum;
            this.BaseNum = inputSource.BaseNum;
            this.Bases.Clear();
            this.Bases.AddRange(inputSource.Bases);
            this.AllInputSignals = new List<Dictionary<int, string>>(inputSource.AllInputSignals);
            for (int i = 0; i < this.Bases.Count; i++)
            {
                var copiedDictionary = inputSource.AllInputSignals[i];
                this.AllInputSignals[i] = new Dictionary<int, string>(copiedDictionary);
            }
        }

        public void CopyValues(ILibraryElement source)
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

        public ushort[] GetProgrammBin()
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

        public void BinProgrammToProperty(ushort[] bin)
        {
            this.BaseNum = bin[0];
            this.InputSignalNum = bin[1];
            this.Connectors[0].ConnectionNumber = bin[2];
        }

        public string StrongName => ProgrammingKeys.INPUT;
    }
}
