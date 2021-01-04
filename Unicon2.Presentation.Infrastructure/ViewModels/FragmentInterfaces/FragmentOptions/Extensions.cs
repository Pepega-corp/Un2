using System.Linq;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions
{
    public static class Extensions
    {
        public static IFragmentOptionCommandViewModel GetCommand(this IFragmentOptionsViewModel fragmentOptionsViewModel, string groupName, string commandName)
        {
            return fragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == groupName).FragmentOptionCommandViewModels
                .First(model => model.TitleKey == commandName);
        }
    }
}