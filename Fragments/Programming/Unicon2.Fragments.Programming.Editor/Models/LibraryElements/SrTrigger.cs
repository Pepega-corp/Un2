using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    public class SrTrigger : ILibraryElement
    {
        public string StrongName => ProgrammingKeys.SR_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;
        public ElementType ElementType => ElementType.SR;
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.EXTENDET_LOGIC;

        public void InitializeDefault()
        {
        }
    }
}
