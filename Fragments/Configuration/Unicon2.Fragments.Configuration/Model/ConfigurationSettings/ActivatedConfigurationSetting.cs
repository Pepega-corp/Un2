using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Model.ConfigurationSettings
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ActivatedConfigurationSetting : IActivatedSetting
    {
        [JsonProperty] public ushort ActivationAddress { get; set; }

        [JsonProperty] public bool IsSettingEnabled { get; set; }

        public async Task<bool> Write()
        {


            return false;
        }

        public string StrongName => ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING;


        public async Task<bool> ApplySetting(ISettingApplyingContext settingApplyingContext)
        {
            if (IsSettingEnabled &&
                settingApplyingContext is IActivatedSettingApplyingContext activatedSettingApplyingContext)
            {
                IQueryResult queryResult = await activatedSettingApplyingContext.DataProvider.WriteSingleCoilAsync(
                    ActivationAddress, true,
                    ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING);
                return queryResult.IsSuccessful;
            }

            return false;
        }

    }
}