using System;
using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IProgrammModel : IDeviceFragment, IDataProviderContaining//, IInitializableFromContainer, ISerializableInFile, ILoadable, IDisposable, IWriteable
    {
        List<ILogicElement> Elements { get; set; }
    }
}