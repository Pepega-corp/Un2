using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements
{
    public interface IDeviceBrowserElement : IStronglyNamed, ILoadable, IDeviceContextConsumer
    {
        IDeviceDirectory ParentDirectory { get; }
        Task<bool> DeleteElementAsync();
        string ElementPath { get; }
        string Name { get; }
    }
}