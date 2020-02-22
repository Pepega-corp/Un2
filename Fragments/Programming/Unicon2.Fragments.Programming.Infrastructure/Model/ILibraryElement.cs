using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface ILibraryElement : IStronglyNamed
    {
        Functional Functional { get; }
        Group Group { get; }
    }
}