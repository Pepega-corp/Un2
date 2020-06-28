using Newtonsoft.Json;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InputEditor : IInputEditor
    {
        public ElementType ElementType => ElementType.In;
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.INPUT_OUTPUT;
        [JsonProperty]
        public List<Dictionary<int, string>> AllInputSignals { get; set; }
        [JsonProperty]
        public List<string> Bases { get; set; }

        public string StrongName => ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;

        public void InitializeDefault()
        {
            this.Bases = new List<string> { "Base0" };

            this.AllInputSignals =
                new List<Dictionary<int, string>>
                {
                    new Dictionary<int, string> {{0, string.Empty}}
                };
        }
    }
}
