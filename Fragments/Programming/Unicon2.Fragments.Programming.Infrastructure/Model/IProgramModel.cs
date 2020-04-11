using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgramModel : IDeviceFragment, IDataProviderContaining, ISerializableInFile//, IInitializableFromContainer, ILoadable, IDisposable, IWriteable
    {
        string ProjectName { get; set; }
        string ProjectPath { get; }
        ISchemeModel[] Schemes { get; set; }
        IConnection[] Connections { get; set; }
    }
}