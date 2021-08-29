using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;

namespace Unicon2.Fragments.Programming.Model.Elements
{
    public class SystemJournal : LogicElement
    {
        private const int BIN_SIZE = 4;
        
        [JsonProperty]
        public List<string> OutputSignals { get; set; }
        [JsonProperty]
        public int OutputSignalNum { get; set; }
        public override ElementType ElementType => ElementType.JS;
        public override string Name => this.ElementType.ToString();
        public override Functional Functional => Functional.BOOLEAN;
        public override Group Group => Group.INPUT_OUTPUT;
        public override int BinSize => BIN_SIZE;
        public override string StrongName => ProgrammingKeys.SYSTEM_JOURNAL;

        public SystemJournal()
        {
            this.OutputSignals = new List<string>();
            this.Connectors = new List<Connector> { new Connector(new Point(), ConnectorOrientation.LEFT, ConnectorType.DIRECT)};
        }

        public override void CopyLibraryValues(ILibraryElement source)
        {
            if (!(source is IJournalEditor journalEditor))
            {
                throw new ArgumentException("Copied source is not " + typeof(SystemJournal));
            }

            this.OutputSignals.Clear();
            this.OutputSignals.AddRange(journalEditor.OutputSignals);
        }
        
        public override void CopyValues(LogicElement source)
        {
            if (source is SystemJournal model)
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
            bindata[0] = 16;
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