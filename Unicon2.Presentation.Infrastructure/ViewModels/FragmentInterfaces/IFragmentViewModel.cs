using System.Threading.Tasks;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentViewModel : IFragmentInitializable, IStronglyNamed
    {
        string NameForUiKey { get; }
        IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        
    }

    public interface IFragmentConnectionChangedListener
    {
        Task OnConnectionChanged();
    }
    public interface IFragmentOpenedListener
    {
        Task SetFragmentOpened(bool isOpened);
    }
    public interface IFragmentFileExtension
    {
        string FileExtension { get; }
    }
    
    public interface IFragmentInitializable
    {
        Task<Result> Initialize(IDeviceFragment deviceFragment);
    }
}