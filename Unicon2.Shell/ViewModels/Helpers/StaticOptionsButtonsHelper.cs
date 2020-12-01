using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.Icons;
using Unicon2.Unity.Commands;

namespace Unicon2.Shell.ViewModels.Helpers
{
    public class StaticOptionsButtonsHelper
    {
        public StaticOptionsButtonsHelper()
        {

        }

        public static void InitializeStaticButtons(ShellViewModel shellViewModel)
        {
            var container = StaticContainer.Container;
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelGettingFunc =
              container.Resolve<Func<IFragmentOptionGroupViewModel>>();
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelGettingFunc =
              container.Resolve<Func<IFragmentOptionCommandViewModel>>();

            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelGettingFunc();
            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = "Save";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDisc;
            fragmentOptionCommandViewModel.OptionCommand = shellViewModel.SaveProjectCommand;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = "Add";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconAdd;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(() =>
            {
                shellViewModel.NavigateToDeviceAddingCommand.Execute(ApplicationGlobalNames.ADDING_MODE);
            });
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            shellViewModel.ToolBarViewModel.StaticOptionsGroup = fragmentOptionGroupViewModel;
        }
    }
}
