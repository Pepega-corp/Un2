﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [DataContract(Namespace = "InputNS")]
    public class Input : IInput
    {
        private const int BIN_SIZE = 3;

        [DataMember]
        public IConnector[] Connectors { get; set; }
        [DataMember]
        public List<Dictionary<int, string>> AllInputSignals { get; set; }
        [DataMember]
        public int InputSignalNum { get; set; }
        [DataMember]
        public List<string> Bases { get; set; }
        [DataMember]
        public int BaseNum { get; set; }
        [DataMember]
        public int ConnectionNumber { get; set; }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
        public ElementType ElementType => ElementType.In;
        public string Name => this.ElementType.ToString();
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.INPUT_OUTPUT;
        public int BinSize => BIN_SIZE;

        public Input()
        {
            this.Connectors = new IConnector[] { new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT) };
            this.Bases = new List<string> {"Base0"};

            this.AllInputSignals =
                new List<Dictionary<int, string>>
                {
                    new Dictionary<int, string> {{0, string.Empty}}
                };
        }

        private Input(Input cloneable) : this()
        {
            this.CopyValues(cloneable);
        }

        public void CopyValues(ILogicElement source)
        {
            if (!(source is Input inputSource))
            {
                throw new ArgumentException("Copied source is not " + typeof(Input));
            }
            
            this.InputSignalNum = inputSource.InputSignalNum;
            this.BaseNum = inputSource.BaseNum;
            this.ConnectionNumber = inputSource.ConnectionNumber;
            this.Bases.Clear();
            this.Bases.AddRange(inputSource.Bases);
            this.AllInputSignals = new List<Dictionary<int, string>>(inputSource.AllInputSignals);
            this.Connectors = new IConnector[inputSource.Connectors.Length];

            for (int i = 0; i < inputSource.Connectors.Length; i++)
            {
                var connector = inputSource.Connectors[i];
                this.Connectors[i] = new Connector(connector.Orientation, connector.Type);
            }

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
            bindata[2] = (ushort)this.ConnectionNumber;
            return bindata;
        }

        public void BinProgrammToProperty(ushort[] bin)
        {
            this.BaseNum = bin[0];
            this.InputSignalNum = bin[1];
            this.ConnectionNumber = bin[2];
        }

        public string StrongName => ProgrammingKeys.INPUT;
    }
}
