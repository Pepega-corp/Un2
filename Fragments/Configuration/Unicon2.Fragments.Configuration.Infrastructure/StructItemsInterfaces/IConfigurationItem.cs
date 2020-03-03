using System;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IConfigurationItem : IDisposable, ICloneable, INameable
    {
        string Description { get; set; }
        T Accept<T>(IConfigurationItemVisitor<T> visitor);
    }
}