using System;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Fragment
{
    public class FragmentPaneViewModel : ViewModelBase, IFragmentPaneViewModel
    {
        private IFragmentViewModel _fragmentViewModel;
        private string _fragmentTitle;


        public FragmentPaneViewModel()
        {
            CloseFragmentCommand = new RelayCommand(OnExecuteCloseFragment);
        }

        private void OnExecuteCloseFragment()
        {
            FragmentPaneClosedAction?.Invoke(this);
        }

        public IFragmentViewModel FragmentViewModel
        {
            get { return _fragmentViewModel; }
            set
            {
                _fragmentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CloseFragmentCommand { get; set; }

        public Action<IFragmentPaneViewModel> FragmentPaneClosedAction { get; set; }

        public string FragmentTitle
        {
            get { return _fragmentTitle; }
            set
            {
                _fragmentTitle = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(WindowNameKey));
            }
        }

        public string WindowNameKey => FragmentTitle;
    }
}
