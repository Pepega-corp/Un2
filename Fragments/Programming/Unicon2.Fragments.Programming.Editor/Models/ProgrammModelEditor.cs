using Newtonsoft.Json;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Programming.Editor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProgrammModelEditor : IProgrammModelEditor
    {
        [JsonProperty] public List<ILibraryElement> Elements { get; set; }
        [JsonProperty] public bool EnableFileDriver { get; set; }
        [JsonProperty] public bool WithHeader { get; set; }
        [JsonProperty] public string LogicHeader { get; set; }
        public string StrongName => ProgrammingKeys.PROGRAMMING;
        public IFragmentSettings FragmentSettings { get; set; }
        public ProgrammModelEditor()
        {
            this.Elements = new List<ILibraryElement>();
        }
    }
}
