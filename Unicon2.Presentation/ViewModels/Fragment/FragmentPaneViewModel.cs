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
            this.CloseFragmentCommand = new RelayCommand(OnExecuteCloseFragment);
        }

        private void OnExecuteCloseFragment()
        {
            this.FragmentPaneClosedAction?.Invoke(this);
        }

        #region Implementation of IFragmentPaneViewModel

        public IFragmentViewModel FragmentViewModel
        {
            get { return this._fragmentViewModel; }
            set
            {
                this._fragmentViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand CloseFragmentCommand { get; set; }

        public Action<IFragmentPaneViewModel> FragmentPaneClosedAction { get; set; }

        public string FragmentTitle
        {
            get { return this._fragmentTitle; }
            set
            {
                this._fragmentTitle = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.WindowNameKey));
            }
        }

        #endregion

        #region Implementation of IDockingWindow

        public string WindowNameKey => this.FragmentTitle;

        #endregion
    }
}
