using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings
{
    public interface IFragmentSetting : IStronglyNamed
    {
        bool IsSettingEnabled { get; set; }
        Task<bool> ApplySetting(ISettingApplyingContext settingApplyingContext);
    }
}