using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements
{
    public interface ILibraryElement : IStronglyNamed
    {
        ElementType ElementType { get; }
        Functional Functional { get; }
        Group Group { get; }

        void InitializeDefault();
    }
}