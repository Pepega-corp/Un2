using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.ViewModels.Fragment.FragmentOptions
{
    public class FragmentOptionToggleCommandViewModel : DefaultFragmentOptionCommandViewModel,
        IFragmentOptionToggleCommandViewModel
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                    OptionCommand?.Execute(value);
                _isChecked = value;
            }
        }
    }
}
