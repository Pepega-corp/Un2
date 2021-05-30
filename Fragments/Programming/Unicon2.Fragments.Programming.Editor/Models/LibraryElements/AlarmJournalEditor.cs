using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AlarmJournalEditor : IJournalEditor
    {
        private const int DEFAULT_SIZE = 32;
        
        public string StrongName => ProgrammingKeys.ALARM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;
        public ElementType ElementType => ElementType.JA;
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.INPUT_OUTPUT;
        [JsonProperty]
        public List<string> OutputSignals { get; set; }
        
        public void InitializeDefault()
        {
            this.OutputSignals = new List<string>();
            for (var i = 0; i < DEFAULT_SIZE; i++)
            {
                this.OutputSignals.Add($"ЖА{i + 1}");
            }
        }
    }
}