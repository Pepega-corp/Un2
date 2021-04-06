using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.Models.LibraryElements
{
    public class OrEditor : IOrEditor
    {
        public string StrongName => ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR;
        public ElementType ElementType => ElementType.Or;
        public Functional Functional => Functional.BOOLEAN;
        public Group Group => Group.SIMPLE_LOGIC;

        public void InitializeDefault()
        {
        }
    }
}
