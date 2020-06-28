using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModelEditor: IDeviceFragment
    {
        List<ILibraryElement> Elements { get; set; }
    }
}