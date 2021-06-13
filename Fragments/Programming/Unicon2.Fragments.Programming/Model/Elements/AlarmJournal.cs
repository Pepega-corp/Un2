using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AlarmJournal : LogicElement, IJournal
    {
        private const int BIN_SIZE = 4;
        
        [JsonProperty]
        public List<string> OutputSignals { get; set; }
        [JsonProperty]
        public int OutputSignalNum { get; set; }
        public override ElementType ElementType => ElementType.JA;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.INPUT_OUTPUT;
        public override int BinSize => BIN_SIZE;
        public override string StrongName => ProgrammingKeys.ALARM_JOURNAL;

        public AlarmJournal()
        {
            this.OutputSignals = new List<string>();
            this.Connectors = new List<IConnector> { new Connector(ConnectorOrientation.LEFT, ConnectorType.DIRECT)};
        }

        public override void CopyLibraryValues(ILibraryElement source)
        {
            if (!(source is IJournalEditor journalEditor))
            {
                throw new ArgumentException("Copied source is not " + typeof(AlarmJournal));
            }

            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(journalEditor.OutputSignals);
        }
        
        public override void CopyValues(ILogicElement source)
        {
            if (source is AlarmJournal model)
            {
                base.CopyValues(source);
                this.OutputSignals = new List<string>(model.OutputSignals);
                this.OutputSignalNum = model.OutputSignalNum;
            }
            else
            {
                throw new Exception($"Source {source} is not Output type");
            }
        }

        public override ushort[] GetProgramBin()
        {
            var bindata = new ushort[BinSize];
            bindata[0] = 17;
            bindata[1] = (ushort)Connectors.First().ConnectionNumber;
            bindata[2] = (ushort)OutputSignalNum;
            bindata[3] = 0x0001;
            return bindata;
        }

        public override void BinProgramToProperty(ushort[] bin)
        {
            this.OutputSignalNum = bin[1];
            this.Connectors[0].ConnectionNumber = bin[2];
        }
    }
}