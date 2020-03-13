using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
    [JsonObject(MemberSerialization.OptIn)]

    public class DefaultFragmentSettings : IFragmentSettings
    {
        public DefaultFragmentSettings()
        {
            FragmentSettings = new List<IFragmentSetting>();
        }


        [JsonProperty]
        public List<IFragmentSetting> FragmentSettings { get; set; }


        public async Task<bool> ApplySettingByKey(string key, ISettingApplyingContext settingApplyingContext)
        {
            IFragmentSetting settingToApply = FragmentSettings.FirstOrDefault((setting => setting.StrongName == key));
            if (settingToApply == null) return false;
            return await settingToApply.ApplySetting(settingApplyingContext);
        }
    }
}