using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public class ControlSignalViewModel : MeasuringElementViewModelBase, IControlSignalViewModel
    {
        private bool? _isCommandSucceed;

        public override string StrongName => MeasuringKeys.CONTROL_SIGNAL +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public ICommand WriteValueCommand { get; set; }

        public bool? IsCommandSucceed
        {
            get { return this._isCommandSucceed; }
            set
            {
                this._isCommandSucceed = value;
                if (this._isCommandSucceed.HasValue)
                {
                    this.ResetDelayed();
                }
                RaisePropertyChanged();
            }
        }

        private async void ResetDelayed()
        {
            await Task.Delay(700);
            this.IsCommandSucceed = null;
        }

        //protected override void SetModel(object model)
        //{
        //    this.WriteValueCommand = new RelayCommand((() =>
        //     {
        //         (model as IControlSignal).Write();
        //     }));
        //    base.SetModel(model);
        //}
    }
}
