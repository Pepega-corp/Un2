using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Model.ConfigurationSettings
{
    [DataContract(Name = nameof(ActivatedConfigurationSetting),Namespace = "ActivatedConfigurationSettingNS")]

    public class ActivatedConfigurationSetting : IActivatedSetting,IWriteable
    {
        [DataMember(Name = nameof(ActivationAddress), Order = 0)]

        public ushort ActivationAddress { get; set; }

        [DataMember(Name = nameof(IsSettingEnabled), Order = 1)]

        public bool IsSettingEnabled { get; set; }

        public async Task<bool> Write()
        {
            if(IsSettingEnabled)
            {
                IQueryResult queryResult= await DataProvider.WriteSingleCoilAsync(ActivationAddress,true,ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING);
                return queryResult.IsSuccessful;
            }
            return false;
        }

        public string StrongName => ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING;
       

        public async Task<bool> ApplySetting(ISettingApplyingContext settingApplyingContext)
        {
         return  await Write();
        }

        public IDataProvider DataProvider { get; set; }
    }
}