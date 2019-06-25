using System;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IConfigurationItem:ILoadable,IWriteable,IInitializableFromContainer,IDisposable, ICloneable, INameable, IStronglyNamed,IAddressRangeableWithDataProvider
    {
        string Description { get; set; }
        void TransferDeviceLocalData(bool isFromDeviceToLocal);
        void InitializeLocalValue(IConfigurationItem localConfigurationItem);
        void InitializeValue(IConfigurationItem localConfigurationItem);
        Action ConfigurationItemChangedAction { get; set; }
        Action InitEditableValueAction { get; set; }
    }
}