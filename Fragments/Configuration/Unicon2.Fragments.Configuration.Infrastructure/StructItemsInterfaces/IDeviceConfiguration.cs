using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IDeviceConfiguration : IDeviceFragment, IDisposable, IDataProviderContaining
    {
        List<IConfigurationItem> RootConfigurationItemList { get; set; }
        bool CheckEquality(IDeviceConfiguration deviceConfigurationToCheck);
        IConfigurationMemory ConfigurationMemory { get; set; }
    }
}