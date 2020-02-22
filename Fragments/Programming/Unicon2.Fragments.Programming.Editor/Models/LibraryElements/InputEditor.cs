using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    [DataContract(Namespace = "InputEditorNS")]
    public class InputEditor : IInputEditor
    {
        public Functional Functional { get; }
        public Group Group { get; }
        [DataMember]
        public List<Dictionary<int, string>> AllInputSignals { get; set; }
        [DataMember]
        public List<string> Bases { get; set; }

        public string StrongName => ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;

        public InputEditor()
        {
            this.Functional = Functional.BOOLEAN;
            this.Group = Group.INPUT_OUTPUT;
            this.Bases = new List<string> { "Base0" };

            this.AllInputSignals =
                new List<Dictionary<int, string>>
                {
                    new Dictionary<int, string> {{0, string.Empty}}
                };
        }


    }
}
