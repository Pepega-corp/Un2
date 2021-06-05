using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    public class RsTrigger : ILibraryElement
    {
        public string StrongName => ProgrammingKeys.RS_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;
        public ElementType ElementType => ElementType.RS;
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.EXTENDET_LOGIC;

        public void InitializeDefault()
        {
        }
    }
}
