using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
    [DataContract(Name = nameof(DefaultFragmentSettings), Namespace = "DefaultFragmentSettingsNS")]

    public class DefaultFragmentSettings : IFragmentSettings
    {
        public DefaultFragmentSettings()
        {
            this.FragmentSettings = new List<IFragmentSetting>();
        }


        [DataMember]
        public List<IFragmentSetting> FragmentSettings { get; set; }


        public async Task<bool> ApplySettingByKey(string key, ISettingApplyingContext settingApplyingContext)
        {
            IFragmentSetting settingToApply = this.FragmentSettings.FirstOrDefault((setting => setting.StrongName == key));
            if (settingToApply == null) return false;
            return await settingToApply.ApplySetting(settingApplyingContext);
        }
    }
}