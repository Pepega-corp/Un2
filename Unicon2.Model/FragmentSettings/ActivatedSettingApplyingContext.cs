using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;

namespace Unicon2.Model.FragmentSettings
{
    public class ActivatedSettingApplyingContext : IActivatedSettingApplyingContext
    {
        public IDataProvider DataProvider { get; set; }
    }
}