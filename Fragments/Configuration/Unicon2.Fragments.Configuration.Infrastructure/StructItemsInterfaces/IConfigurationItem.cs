using System;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IConfigurationItem : IDisposable, ICloneable, INameable
    {
        string Description { get; set; }
        T Accept<T>(IConfigurationItemVisitor<T> visitor);
    }
}