using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
    [DataContract(Namespace = "QuickMemoryAccessSettingNS")]
    public class QuickMemoryAccessSetting : IQuickMemoryAccessSetting
    {
        private IDataProvider _dataProvider;
        private bool _isInitialized;

        public QuickMemoryAccessSetting()
        {
            this.QuickAccessAddressRanges = new List<IRange>();
        }


        public string StrongName => ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING;

        [DataMember]
        public bool IsSettingEnabled { get; set; }
        public async Task<bool> ApplySetting(ISettingApplyingContext context)
        {
           var settingApplyingContext = context as IQuickAccessMemoryApplyingContext;
            foreach (var quickAccessAddressRange in QuickAccessAddressRanges)
            {
                await settingApplyingContext.OnFillAddressRange(quickAccessAddressRange);
            }
            return this.IsSettingEnabled;
        }

        [DataMember]
        public List<IRange> QuickAccessAddressRanges { get; set; }
        
    }
}
