using System;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.ViewModels;

namespace Unicon2.Presentation.Services
{
    public class ToolBarService : IToolBarService
    {
        private readonly ToolBarViewModel _toolBarViewModel;

        public ToolBarService(ToolBarViewModel toolBarViewModel)
        {
            _toolBarViewModel = toolBarViewModel;

        }


        public void SetCurrentFragmentToolbar(IFragmentOptionsViewModel fragmentOptionsViewModel)
        {
            throw new NotImplementedException();
        }
    }
}