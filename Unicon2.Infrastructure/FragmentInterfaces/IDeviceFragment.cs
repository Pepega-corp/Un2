using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces
{
    public interface IDeviceFragment : IStronglyNamed
    {
        IFragmentSettings FragmentSettings { get; set; }
    }
}