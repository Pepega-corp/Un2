using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels
{
    public class ToolBarViewModel: ViewModelBase
    {
        private IFragmentOptionGroupViewModel _dynamicOptionsGroup;
        public IFragmentOptionGroupViewModel StaticOptionsGroup { get; set; }

        public IFragmentOptionGroupViewModel DynamicOptionsGroup => _dynamicOptionsGroup;

        public void SetDynamicOptionsGroup(IFragmentOptionGroupViewModel dynamicOptionsGroupViewModel)
        {
            _dynamicOptionsGroup = dynamicOptionsGroupViewModel;
            RaisePropertyChanged(nameof(DynamicOptionsGroup));
        }
    }
}