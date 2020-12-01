using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgramModel : IDeviceFragment //, IInitializableFromContainer, ILoadable, IDisposable, IWriteable
    {
        string ProjectName { get; set; }
        string ProjectPath { get; }
        ISchemeModel[] Schemes { get; set; }
        IConnection[] Connections { get; set; }
    }
}