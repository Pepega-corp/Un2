using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModelEditor: IDeviceFragment
    {
        List<ILibraryElement> Elements { get; set; }
        bool EnableFileDriver { get; set; }
        bool WithHeader { get; set; }
        string LogicHeader { get; set; }
    }
}