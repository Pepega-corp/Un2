using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModel : IDeviceFragment, IDataProviderContaining//, IInitializableFromContainer, ISerializableInFile, ILoadable, IDisposable, IWriteable
    {
        string ProjectName { get; set; }
        ISchemeModel[] Schemes { get; set; }
    }
}