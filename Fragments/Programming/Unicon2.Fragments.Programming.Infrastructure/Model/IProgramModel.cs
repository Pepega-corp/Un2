using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgramModel : IDeviceFragment
    {
        string ProjectName { get; set; }
        string ProjectPath { get; }
        List<ISchemeModel> Schemes { get; }
        List<IConnection> Connections { get; }
        bool EnableFileDriver { get; set; }
        bool WithHeader { get; set; }
        string LogicHeader { get; set; }
        int LogBinSize { get; set; }
    }
}