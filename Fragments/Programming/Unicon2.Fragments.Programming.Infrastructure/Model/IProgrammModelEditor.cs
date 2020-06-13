using Unicon2.Infrastructure.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModelEditor: IDeviceFragment
    {
        ILibraryElement[] Elements { get; set; }
    }
}