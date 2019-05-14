using System;
using System.Windows;
using System.Windows.Input;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels
{
    public class UniconSplashScreenViewModel : ViewModelBase
    {
        private string _status;


        public UniconSplashScreenViewModel(App application)
        {
            application.BootsrapperMessageAction += this.OnBootstrapperMessage;
            this.CloseCommand=new RelayCommand<object>(o =>
            {
                (o as Window)?.Close();
            });
        }

        public ICommand CloseCommand { get; }

        public string Status
        {
            get { return this._status; }
            set
            {
                this._status = value;
                RaisePropertyChanged();
            }
        }

        private void OnBootstrapperMessage(string message)
        {
            this.Status += string.Concat(Environment.NewLine, message, "...");
        }
    }
}
