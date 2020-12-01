using System.Collections.Generic;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties
{
    public interface IProperty : IConfigurationItem, IUshortFormattable, IMeasurable, IRangeable
    {
        ushort Address { get; set; }
        ushort NumberOfPoints { get; set; }
        ushort NumberOfWriteFunction { get; set; }
        List<IDependency> Dependencies { get; set; }
    }
}