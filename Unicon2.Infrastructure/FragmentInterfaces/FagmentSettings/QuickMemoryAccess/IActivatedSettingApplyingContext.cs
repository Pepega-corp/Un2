using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
    public interface IActivatedSettingApplyingContext:ISettingApplyingContext
    {
        IDataProvider DataProvider { get; set; }
    }
}