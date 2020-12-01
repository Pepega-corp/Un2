using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model
{
	public interface IFileBrowser : IDeviceFragment
	{
		IDeviceDirectory RootDeviceDirectory { get; }
		Task LoadRootDirectory();
	}
}