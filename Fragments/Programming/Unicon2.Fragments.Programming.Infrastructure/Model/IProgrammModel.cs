using System;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModel : IDeviceFragment, IDataProviderContaining//, IInitializableFromContainer, ISerializableInFile, ILoadable, IDisposable, IWriteable
    {
        
    }
}