using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IDataProviderSetter
    {
        void SetDataProvider(IDataProviderContainer dataProvider);
    }
}