using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IDeviceConfiguration : IDeviceFragment, IDisposable
    {
        List<IConfigurationItem> RootConfigurationItemList { get; set; }
        bool CheckEquality(IDeviceConfiguration deviceConfigurationToCheck);
    }
}