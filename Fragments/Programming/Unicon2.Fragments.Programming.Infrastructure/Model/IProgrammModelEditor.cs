using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModelEditor: IStronglyNamed
    {
        ILibraryElement[] Elements { get; set; }
    }
}