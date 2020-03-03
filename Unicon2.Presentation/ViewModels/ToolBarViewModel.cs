using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels
{
    public class ToolBarViewModel : ViewModelBase
    {
        private IFragmentOptionsViewModel _dynamicOptions;
        public IFragmentOptionGroupViewModel StaticOptionsGroup { get; set; }

        public IFragmentOptionsViewModel DynamicOptionsGroup => _dynamicOptions;

        public void SetDynamicOptionsGroup(IFragmentOptionsViewModel dynamicOptionsViewModel)
        {
            if (dynamicOptionsViewModel == null)
            {
                _dynamicOptions = null;
            }

            _dynamicOptions = dynamicOptionsViewModel;
            RaisePropertyChanged(nameof(DynamicOptionsGroup));
        }
    }
}