using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IToolBarService
    {
        void SetCurrentFragmentToolbar(IFragmentOptionsViewModel fragmentOptionsViewModel);
    }
}
