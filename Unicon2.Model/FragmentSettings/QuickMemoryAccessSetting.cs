using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class QuickMemoryAccessSetting : IQuickMemoryAccessSetting
    {
        private bool _isInitialized;

        public QuickMemoryAccessSetting()
        {
            QuickAccessAddressRanges = new List<IRange>();
        }


        public string StrongName => ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING;

        [JsonProperty]
        public bool IsSettingEnabled { get; set; }
        public async Task<bool> ApplySetting(ISettingApplyingContext context)
        {
           var settingApplyingContext = context as IQuickAccessMemoryApplyingContext;
            foreach (var quickAccessAddressRange in QuickAccessAddressRanges)
            {
                await settingApplyingContext.OnFillAddressRange(quickAccessAddressRange);
            }
            return IsSettingEnabled;
        }

        [JsonProperty]
        public List<IRange> QuickAccessAddressRanges { get; set; }
        
    }
}
