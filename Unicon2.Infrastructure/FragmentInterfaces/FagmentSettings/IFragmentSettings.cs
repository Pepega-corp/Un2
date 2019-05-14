using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings
{
    public interface IFragmentSettings
    {
        List<IFragmentSetting> FragmentSettings { get; set; }
        Task<bool> ApplySettingByKey(string key,ISettingApplyingContext settingApplyingContext);
    }
}