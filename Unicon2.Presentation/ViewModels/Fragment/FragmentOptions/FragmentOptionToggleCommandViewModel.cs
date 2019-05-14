using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.ViewModels.Fragment.FragmentOptions
{
    public class FragmentOptionToggleCommandViewModel : DefaultFragmentOptionCommandViewModel,
        IFragmentOptionToggleCommandViewModel
    {
        private bool _isChecked;
        
        #region Implementation of IFragmentOptionToggleCommandViewModel

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

        #endregion
    }
}
