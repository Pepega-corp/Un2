using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IDeviceConfiguration : IDeviceFragment, ISerializableInFile, IInitializableFromContainer,ILoadable, IDisposable,IWriteable
    {
        List<IConfigurationItem> RootConfigurationItemList { get; set; }
        bool CheckEquality(IDeviceConfiguration deviceConfigurationToCheck);
    }
}