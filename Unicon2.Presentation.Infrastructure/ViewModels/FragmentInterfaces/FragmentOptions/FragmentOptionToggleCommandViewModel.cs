using System;
using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions
{
    public class FragmentOptionToggleCommandViewModel : DefaultFragmentOptionCommandViewModel
    {
        private readonly Func<bool> _canExecute;

        public FragmentOptionToggleCommandViewModel(ICommand command, Func<bool> canExecute = null)
        {
            _canExecute = canExecute;
            OptionCommand = command;
        }

        private bool _isChecked;
        private bool _isEnabled;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                    OptionCommand?.Execute(value);
                _isChecked = value;
                RaisePropertyChanged();
            }
        }


        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value; 
                RaisePropertyChanged();
            }
        }

        public override void UpdateAvailability()
        {
            if (_canExecute !=null && !_canExecute())
            {
                IsEnabled = false;
            }
            else
            {
                IsEnabled = true;
            }
            
            base.UpdateAvailability();
        }
    }
}
