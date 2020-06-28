using Newtonsoft.Json;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OutputEditor : IOutputEditor
    {
        private const int DEFAULT_SIZE = 32;

        public string StrongName => ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;

        public ElementType ElementType =>  ElementType.Out;

        public Functional Functional=>Functional.BOOLEAN;

        public Group Group =>Group.INPUT_OUTPUT;
        [JsonProperty]
        public List<string> OutputSignals { get; set; }

        public void InitializeDefault()
        {
            this.OutputSignals = new List<string>();
            for (int i = 0; i < DEFAULT_SIZE; i++)
            {
                this.OutputSignals.Add($"ССЛ{i + 1}");
            }
        }
    }
}
