using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    [DataContract(Namespace = "OutputEditorNS")]
    class OutputEditor : IOutputEditor
    {
        private const int DEFAULT_SIZE = 32;

        public string StrongName => ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;

        public Functional Functional { get; }

        public Group Group { get; }
        [DataMember]
        public List<string> OutputSignals { get; set; }

        public OutputEditor()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;
            this.OutputSignals = new List<string>();
            for (int i = 0; i < DEFAULT_SIZE; i++)
            {
                this.OutputSignals.Add($"ССЛ{i + 1}");
            }
        }
    }
}
