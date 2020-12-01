using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions
{
    public interface IFragmentOptionCommandViewModel
    {
        string TitleKey { get; set; }
        ICommand OptionCommand { get; set; }
        string IconKey { get; set; }
        void UpdateAvailability();
    }
    

}