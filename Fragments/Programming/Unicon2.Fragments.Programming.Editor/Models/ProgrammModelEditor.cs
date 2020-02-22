using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models
{
    [DataContract(Namespace = "ProgrammModelEditorNS")]
    public class ProgrammModelEditor : IProgrammModelEditor
    {
        [DataMember]
        public ILibraryElement[] Elements { get; set; }

        public string StrongName => ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;
    }
}
