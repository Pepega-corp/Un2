using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.Commands;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.Commands
{
    public class WriteDiscretCommand : ICommandFactory
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


        private bool CanExecute()
        {
            return true;
        }

        private async void Execute()
        {
            if (this._controlSignal.WritingValueContext.NumberOfFunction == 5)
            {
                var result = await this._deviceContext.DataProviderContainer.DataProvider.WriteSingleCoilAsync(
                    _controlSignal.WritingValueContext.Address,
                    this._controlSignal.WritingValueContext.ValueToWrite == 1,
                    "Write control signal" + this._controlSignal.Name);

                this._controlSignalViewModel.IsCommandSucceed = result.IsSuccessful;

            }
        }

        public ICommand CreateCommand()
        {
            return new RelayCommand(execute: Execute, canExecute: CanExecute);
        }
    }
}