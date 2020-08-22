using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.FileOperations
{
    public interface IDataProviderSetter
    {
        void SetDataProvider(IDataProvider dataProvider);
    }
}