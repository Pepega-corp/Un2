using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.Commands
{
    public class WriteDiscretCommand : ICommand
    {
        private readonly DeviceContext _deviceContext;
        private readonly IControlSignal _controlSignal;
        private readonly IControlSignalViewModel _controlSignalViewModel;

        public WriteDiscretCommand(DeviceContext deviceContext, IControlSignal controlSignal,
            IControlSignalViewModel controlSignalViewModel)
        {
            this._deviceContext = deviceContext;
            this._controlSignal = controlSignal;
            this._controlSignalViewModel = controlSignalViewModel;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (this._controlSignal.WritingValueContext.NumberOfFunction == 5)
            {
                var result = await this._deviceContext.DataProviderContaining.DataProvider.WriteSingleCoilAsync(
                    _controlSignal.WritingValueContext.Address,
                    this._controlSignal.WritingValueContext.ValueToWrite == 1,
                    "Write control signal" + this._controlSignal.Name);

                this._controlSignalViewModel.IsCommandSucceed = result.IsSuccessful;

            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
